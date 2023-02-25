var path = require('path');

var HtmlwebpackPlugin = require('html-webpack-plugin');
var ROOT_PATH = path.resolve(__dirname);
var SRC_PATH = path.resolve(ROOT_PATH, 'src', 'index.ts');
var BUILD_PATH = path.resolve(ROOT_PATH, 'build');

module.exports = {
    entry: SRC_PATH,
    mode : 'development',
    module: {
        rules: [
            {
                test: /\.tsx?$/,
                use: 'ts-loader',
                exclude: /node_modules/,
            },
            {
                test: /\.glsl?$/,
                use: 'raw-loader',
                exclude: /node_modules/,
            }
        ],
    },
    resolve: {
        extensions: ['.tsx', '.ts', '.js', '.glsl'],
    },
    output: {
        path: BUILD_PATH,
        filename: 'main.js'
    },
    plugins: [
        new HtmlwebpackPlugin({title: 'Webnity'})
    ],
    devServer: { 
        host: 'localhost',
        port: 5000,
        open: true,
        hot: true,
        compress: true
    }
};