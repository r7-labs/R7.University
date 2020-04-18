class WorkbookConverter extends React.Component {

    constructor (props) {
        super (props);
        this.state = {
            error: false,
            pingLabel: "Ping!"
        };
    }
    
    render() {
        return (
            <div class="form-group">
                <input type="button" className="btn btn-primary" onClick={this.ping.bind(this)} value={this.state.pingLabel} /> 
            </div>
        );
    }
    
    ping (e) {
        e.preventDefault ();
        this.props.service.ping (
            (retData) => {
                this.setState ({
                    error: false,
                    pingLabel: retData.pingLabel
                });
            },
            (xhr, status) => {
                console.log (xhr);
                console.log (status);
                this.setErrorState ();
            }
        );
    }
    
    setErrorState () {
        this.setState ({
            error: true,
            pingLabel: "Error!"
        });
    }
}

(function ($, window, document) {

    class WorkbookConverterService {
        constructor (moduleId) {
            this.moduleId = moduleId;
            this.baseServicepath = $.dnnSF (moduleId).getServiceRoot ("R7.University.Launchpad");
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
            }).fail (function (xhr, status) {
                if (fail != undefined) {
                    fail (xhr, status);
                }
            });
        }
        
        ping (success, fail) {
            this.ajaxCall ("GET", "WorkbookConverter", "Ping", null, null, success, fail);
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