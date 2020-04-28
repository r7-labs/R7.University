class WorkbookDownloader extends React.Component {
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
                <a role="button" class="btn btn-outline-secondary"
                        href={"/DesktopModules/R7.University.Employees/API/Employee/Export?employeeId=" + this.props.employeeId + "&format=CSV"}>
                    <i class="fas fa-file-csv" aria-hidden="true"></i>
                    Export to .CSV
                </a>
            );
        }
        return null;
    }
    
    renderActions () {
        if (this.props.isAuthenticated === true) {
            return (
                <div>
                    <a role="button" class="btn btn-outline-primary"
                            href={"/DesktopModules/R7.University.Employees/API/Employee/Export?employeeId=" + this.props.employeeId + "&format=Excel"}>
                        <i className="fas fa-file-excel" aria-hidden="true"></i>
                        Export to .XLSX
                    </a>
                    {this.renderAdminActions ()}
                </div>
            );
        }
        return (
            <p className="alert alert-warning">
                Currently only logged in users can download employee data as a spreadsheet (.XLSX) file!
                Please <a href={this.props.loginUrl} target="_blank">login</a> to the website, reload the page and try again.
            </p>
        );
    }
    
    render () {
        return (
            <div>
                <p className="alert alert-info">
                    You can download employee data as a spreadsheet (.XLSX) file
                    in order to edit the data offline using the e.g. Microsoft Excel or Libreoffice Calc
                    and then send the updated form to the authorized website editor.  
                </p>
                {this.renderActions ()}
            </div>
        );
    }
}

// basic export
window.WorkbookDownloader = WorkbookDownloader;
