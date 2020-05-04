class EmployeeExporter extends React.Component {
    constructor (props) {
        super (props);
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
                    <a role="button" className="btn btn-outline-secondary"
                            href={"/DesktopModules/R7.University.Employees/API/Employee/ExportToCsv?employeeId=" + this.props.employeeId}>
                        <i className="fas fa-file-csv mr-2" aria-hidden="true"></i> {this.getString ("ExportToCSV")}
                    </a>
                </li>
            );
        }
        return null;
    }
    
    renderActions () {
        if (this.props.isAuthenticated === true) {
            return (
                <ul className="list-inline">
                    <li className="list-inline-item">
                        <a role="button" className="btn btn-outline-primary"
                                href={"/DesktopModules/R7.University.Employees/API/Employee/ExportToExcel?employeeId=" + this.props.employeeId}>
                            <i className="fas fa-file-excel mr-2" aria-hidden="true"></i> {this.getString ("ExportToExcel")}
                        </a>
                    </li>    
                    {this.renderAdminActions ()}
                </ul>
            );
        }
        return (
            <p className="alert alert-warning" dangerouslySetInnerHTML={{__html: this.getString ("Unauthorized").replace ("{{loginUrl}}", this.props.loginUrl)}}></p>
        );
    }
    
    render () {
        return (
            <div>
                <p className="alert alert-info">{this.getString ("About")}</p>
                {this.renderActions ()}
            </div>
        );
    }
}

// basic export
window.EmployeeExporter = EmployeeExporter;
