class WorkbookConverterError {
    constructor () {
        this.level = 0;
        this.title = "";
        this.message = "";
    }
}

class WorkbookConverterState {
    constructor () {
        this.error = null;
        this.files = [];
    }
}

class WorkbookConverter extends React.Component {

    constructor (props) {
        super (props);
        this.state = new WorkbookConverterState ();
    }
    
    renderFile (file, index) {
        return (
            <tr>
                <td>{index + 1}</td>
                <td>{file.fileName}</td>
                <td><a href={this.props.service.getUrl ("WorkbookConverter", "Convert", null)
                    + "?fileName=" + encodeURIComponent (file.fileName)
                    + "&tempFileName=" + encodeURIComponent (file.tempFileName)
                    + "&format=csv"}>CSV</a></td>
            </tr>
        );
    }
    
    renderFiles (files) {
        if (files.length > 0) {
            return (
                <table className="table table-bordered table-striped table-hover">
                    <thead>
                        <tr>
                            <th>#</th>
                            <th>File Name</th>
                            <th>Download</th>
                        </tr>
                    </thead>
                    <tbody>
                        {files.map ((f, i) => this.renderFile (f, i))}
                    </tbody>
                </table>
            );
        }
        return null;
    }
    
    getBsAlertName (errorLevel) {
        if (errorLevel === 1) {
            return "danger";
        }
        if (errorLevel === 2) {
            return "warning";
        }
        return "info";
    }
    
    renderError (error) {
        if (error !== null) {
            return (
                <div className={"alert alert-" + this.getBsAlertName (error.level)} role="alert">
                    <h4 className="alert-heading">{error.title}</h4>
                    <p className="mb-0">{error.message}</p>
                </div>
            );
        }
        return null;
    }
    
    render () {
        return (
            <div>
                {this.renderError (this.state.error)}
                <form>
                    <fieldset>
                        <div className="form-group">
                            <label for={"u8y_wbc_upload_" + this.props.moduleId}>Select .XLSX file(s)</label>
                            <input type="file" className="form-control-file" id={"u8y_wbc_upload_" + this.props.moduleId}
                                accept=".xlsx, application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                                multiple="multiple"
                            />
                        </div>    
                        <div className="form-group">    
                            <a role="button" className="btn btn-primary" href="#" onClick={this.upload.bind(this)}>
                                <i className="fas fa-upload"></i> Upload
                            </a>
                        </div>
                    </fieldset>
                </form>
                {this.renderFiles (this.state.files)}
            </div>
        );
    }
    
    upload (e) {
        e.preventDefault ();
       
        const newState = new WorkbookConverterState ();
        
        const formData = new FormData ();
        var fileInput = document.getElementById ("u8y_wbc_upload_" + this.props.moduleId);
        for (let i = 0; i < fileInput.files.length; i++) {
            formData.append ("file" + i, fileInput.files [i]);
        }
        
        this.props.service.upload (
            formData,    
            (results) => {
                for (let result of results) {
                    newState.files.push ({
                        tempFileName: result.tempFileName,
                        fileName: result.fileName
                    });
                }            
                this.setState (newState);
            },
            (xhr, status, err) => {
                console.log (xhr);
                console.log (status);
                console.log (err);
                
                newState.error = new WorkbookConverterError ();
                newState.error.level = 1;
                newState.error.title = xhr.status + " " + xhr.statusText;
                newState.error.message = "Error uploading file(s). You can reload the page and try again. If problem persists, please contact the system administrator.";
                this.setState (newState);
            }
        );
    }
}

(function ($, window, document) {

    class WorkbookConverterService {
        constructor (moduleId) {
            this.moduleId = moduleId;
            this.baseServicepath = $.dnnSF (moduleId).getServiceRoot ("R7.University.Launchpad");
        }
        
        getUrl (controller, action, id) {
            return this.baseServicepath + controller + "/" + action + (id != null ? "/" + id : "");
        }
    
        ajaxCall (type, controller, action, id, data, success, fail) {
            $.ajax ({
                type: type,
                url: this.baseServicepath + controller + "/" + action + (id != null ? "/" + id : ""),
                beforeSend: $.dnnSF (this.moduleId).setModuleHeaders,
                data: data
            }).done (function (retData) {
                if (success != undefined) {
                    success (retData);
                }
            }).fail (function (xhr, status, err) {
                if (fail != undefined) {
                    fail (xhr, status, err);
                }
            });
        }
        
        ajaxCallUnprocessed (type, controller, action, id, data, success, fail) {
            $.ajax ({
                type: type,
                url: this.baseServicepath + controller + "/" + action + (id != null ? "/" + id : ""),
                beforeSend: $.dnnSF (this.moduleId).setModuleHeaders,
                processData: false,
                contentType: false,
                data: data
            }).done (function (retData) {
                if (success != undefined) {
                    success (retData);
                }
            }).fail (function (xhr, status, err) {
                if (fail != undefined) {
                    fail (xhr, status, err);
                }
            });
        }
        
        upload (data, success, fail, always) {
            this.ajaxCallUnprocessed ("POST", "WorkbookConverter", "Upload", null, data, success, fail);
        }
    }

    $(() => {
        $(".u8y-workbook-converter").each ((i, m) => {
            const moduleId = $(m).data ("module-id");
            ReactDOM.render (<WorkbookConverter
                moduleId={moduleId}
                service={new WorkbookConverterService (moduleId)}
            />, m);
        });
    });
}) (jQuery, window, document);