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
                    <a role="button" class="btn btn-outline-secondary"
                            href={"/DesktopModules/R7.University.Employees/API/Employee/Export?employeeId=" + this.props.employeeId + "&format=CSV"}>
                        <i class="fas fa-file-csv" aria-hidden="true"></i>
                        {this.getString ("ExportToCSV")}
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
                        <a role="button" class="btn btn-outline-primary"
                                href={"/DesktopModules/R7.University.Employees/API/Employee/Export?employeeId=" + this.props.employeeId + "&format=Excel"}>
                            <i className="fas fa-file-excel" aria-hidden="true"></i>
                            {this.getString ("ExportToExcel")}
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
