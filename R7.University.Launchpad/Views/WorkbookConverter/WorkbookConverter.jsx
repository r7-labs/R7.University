class WorkbookConverter extends React.Component {
    render() {
        return <p>Hello, world from DNN module {this.props.moduleId}!</p>;
    }
}

(function ($, window, document) {
    $(() => {
        $(".u8y-workbook-converter").each ((i, m) => {
            ReactDOM.render (<WorkbookConverter
                moduleId={$(m).data ("module-id")}
            />, m);
        });
    });
}) (jQuery, window, document);