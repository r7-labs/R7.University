class EmployeeExporter extends React.Component {
    constructor (props) {
        super (props);
        this.recaptcha = React.createRef ();
        this.state = {
            recaptchaVerified: false,
            recaptchaError: this.needShowRecaptcha () && (!this.recaptchaObjectExists () || !this.recaptchaSitekeyOk ())
        }
    }

    recaptchaSitekeyOk () {
        if (typeof (this.props.recaptchaSitekey) !== "undefined"
            && this.props.recaptchaSitekey !== null
            && this.props.recaptchaSitekey.trim().length > 0) {
            return true;
        }
        return false;
    }

    recaptchaObjectExists () {
        if (typeof (window.grecaptcha) === "object") {
            return true;
        }
        return false;
    }

    needShowRecaptcha () {
        return this.props.isAuthenticated === false;
    }

    canShowRecaptcha () {
        return this.recaptchaSitekeyOk () && !this.state.recaptchaError;
    }

    canShowActions () {
        return !this.needShowRecaptcha () || this.state.recaptchaVerified === true;
    }

    getString (key) {
        if (key.includes (".")) {
            return this.props.resources [key];
        }
        return this.props.resources [key + ".Text"]
    }

    renderAdminActions () {
        if (this.props.isAdmin === true) {
            return (
                <li className="list-inline-item">
                    <a role="button" className="btn btn-outline-success"
                            href={"/DesktopModules/R7.University.Employees/API/Employee/ExportToCsv?employeeId=" + this.props.employeeId}>
                        <i className="fas fa-file-csv mr-2" aria-hidden="true"></i> {this.getString ("ExportToCSV")}
                    </a>
                </li>
            );
        }
        return null;
    }

    renderActions () {
        if (this.canShowActions ()) {
            return (
                <ul className="list-inline mb-3">
                    <li className="list-inline-item">
                        <a role="button" className="btn btn-success"
                                href={"/DesktopModules/R7.University.Employees/API/Employee/ExportToExcel?employeeId=" + this.props.employeeId}>
                            <i className="fas fa-file-excel mr-2" aria-hidden="true"></i> {this.getString ("ExportToExcel")}
                        </a>
                    </li>
                    {this.renderAdminActions ()}
                </ul>
            );
        }
        return null;
    }

    renderError () {
        if (this.state.recaptchaError === true) {
            return (
                <p className="alert alert-danger"
                    dangerouslySetInnerHTML={{__html: this.getString ("RecaptchaError").replace ("{{loginUrl}}", this.props.loginUrl)}}>
                </p>
            );
        }
        return null;
    }

    renderRecaptcha () {
        if (this.needShowRecaptcha () && this.canShowRecaptcha ()) {
            return (
                <div ref={this.recaptcha} className="mb-3"></div>
            );
        }
        return null;
    }

    componentDidMount () {
        if (this.needShowRecaptcha () && this.canShowRecaptcha ()) {
            grecaptcha.render (this.recaptcha.current, {
                "sitekey": this.props.recaptchaSitekey,
                "callback": () => {
                    this.setState ({
                        recaptchaVerified: true,
                        recaptchaError: false
                    });
                },
                "error-callback": () => {
                    this.setState ({
                        recaptchaVerified: false,
                        recaptchaError: true
                    });
                },
                "expired-callback": () => {
                    this.setState ({
                        recaptchaVerified: false,
                        recaptchaError: false
                    });
                }
            });
        }
    }

    render () {
        return (
            <div>
                <p className="alert alert-info">{this.getString ("About")}</p>
                {this.renderError ()}
                {this.renderRecaptcha ()}
                {this.renderActions ()}
            </div>
        );
    }
}

// basic export
window.EmployeeExporter = EmployeeExporter;
