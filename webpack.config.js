var ExtractTextPlugin = require ("extract-text-webpack-plugin");
var path = require ("path");

var scssConfig = {
    mode: "production",
    entry: {
        test: "./R7.University/assets/css/test.scss"
    },
    output: {
        path: path.resolve (__dirname, "R7.University/assets/css"),
        // TODO: suppress JS output somehow?
        filename: "[name].webpack.tmp"
    },
    module: {
        rules: [
            {
                test: /\.scss$/,
                exclude: /(node_modules|bower_components)/,
                use: ExtractTextPlugin.extract ({
                    use: [{
                        loader: "css-loader",
                        options: {url: false}
                    },
                    "sass-loader"]
                })
            }
        ]
    },
    plugins: [
        new ExtractTextPlugin ({
            filename: "[name].min.css",
            allChunks: true
        })
    ]
};

var jsxConfig = {
    mode: "production",
    entry: {
        test: "./R7.University/assets/js/test.jsx",
        WorkbookConverter: "./R7.University.Launchpad/Views/WorkbookConverter/WorkbookConverter.jsx",
        EmployeeExporter: "./R7.University.Employees/Views/EmployeeExporter/EmployeeExporter.jsx"
    },
    output: {
        path: path.resolve (__dirname, "R7.University/assets/js"),
        filename: "[name].min.js"
    },
    module: {
        rules: [
            {
                test: /\.js$|\.jsx$/,
                exclude: /(node_modules|bower_components)/,
                use: {
                    loader: "babel-loader",
                    options: {
                        presets: ["@babel/preset-react", "@babel/preset-env"]
                    }
                }
            }
        ]
    },
};

var jsConfig = {
    mode: "production",
    entry: {
        EditAchievements: "./R7.University.Controls/js/EditAchievements.js"
    },
    output: {
        path: path.resolve (__dirname, "R7.University/assets/js"),
        filename: "[name].min.js"
    },
    module: {
        rules: [
            {
                test: /\.js$/,
                exclude: /(node_modules|bower_components)/,
                use: {
                    loader: "babel-loader",
                    options: {
                        presets: ["@babel/preset-env"]
                    }
                }
            }
        ]
    },
};

module.exports = [scssConfig, jsxConfig, jsConfig];
