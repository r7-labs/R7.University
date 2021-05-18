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

    getString (key) {
        if (key.includes (".")) {
            return this.props.resources [key];
        }
        return this.props.resources [key + ".Text"]
    }

    renderFile (file, index) {
        return (
            <tr>
                <td>{index + 1}</td>
                <td>{file.fileName}</td>
                <td><a href={this.props.service.getUrl ("WorkbookConverter", "Convert", null)
                    + "?fileName=" + encodeURIComponent (file.fileName)
                    + "&guid=" + encodeURIComponent (file.guid)
                    + "&format=LinearCSV"}>CSV</a>
                    <span className="mx-2">|</span>
                    <a href={this.props.service.getUrl ("WorkbookConverter", "Convert", null)
                    + "?fileName=" + encodeURIComponent (file.fileName)
                    + "&guid=" + encodeURIComponent (file.guid)
                    + "&format=LinearCSV_270"} className="text-muted">CSV (2.7.0)</a>
                </td>
                <td>
                    <a href={this.props.service.getUrl ("WorkbookConverter", "ConvertOriginal", null)
                    + "?fileName=" + encodeURIComponent (file.fileName)
                    + "&guid=" + encodeURIComponent (file.guid)
                    + "&format=LinearCSV"}>CSV</a>
                    <span className="d-none">
                        <span className="mx-2">|</span>
                        <a href={this.props.service.getUrl ("WorkbookConverter", "ConvertOriginal", null)
                        + "?fileName=" + encodeURIComponent (file.fileName)
                        + "&guid=" + encodeURIComponent (file.guid)
                        + "&format=LinearCSV_270"} className="text-muted">CSV (2.7.0)</a>
                    </span>
                </td>
            </tr>
        );
    }

    renderFiles (files) {
        if (files.length > 0) {
            return (
                <table className="table table-bordered table-striped table-hover">
                    <thead>
                        <tr>
                            <th>{this.getString ("Number")}</th>
                            <th>{this.getString ("FileName")}</th>
                            <th>{this.getString ("Download")}</th>
                            <th>{this.getString ("DownloadOriginal")}</th>
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
                            <label for={"u8y_wbc_upload_" + this.props.moduleId}>{this.getString ("SelectFiles")}</label>
                            <input type="file" className="form-control-file" id={"u8y_wbc_upload_" + this.props.moduleId}
                                accept=".xls, application/vnd.ms-excel"
                                multiple="multiple"
                            />
                        </div>
                        <div className="form-group">
                            <a role="button" className="btn btn-primary" href="#" onClick={this.upload.bind(this)}>
                                <i className="fas fa-upload"></i> {this.getString ("Upload")}
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
                        guid: result.guid,
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
                newState.error.message = this.getString ("UploadErrorMessage");
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

        ajaxCall (type, url, data, success, fail) {
            // TODO: Review contentType could be boolean?
            this.ajaxCall (type, url, data, true, true, success, fail);
        }

        ajaxCall (type, url, data, processData, contentType, success, fail) {
            $.ajax ({
                type: type,
                url: url,
                beforeSend: $.dnnSF (this.moduleId).setModuleHeaders,
                processData: processData,
                contentType: contentType,
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

        upload (data, success, fail) {
            this.ajaxCall ("POST", this.getUrl ("WorkbookConverter", "Upload", null), data, false, false, success, fail);
        }
    }

    $(() => {
        $(".u8y-workbook-converter-react-root").each ((i, m) => {
            const moduleId = $(m).data ("module-id");
            ReactDOM.render (<WorkbookConverter
                moduleId={moduleId}
                resources={$(m).data ("resources")}
                service={new WorkbookConverterService (moduleId)}
            />, m);
        });
    });
}) (jQuery, window, document);
