const path = require('path')

module.exports = {
    mode: 'development',
    devServer: {
        static: {
            directory: path.join(__dirname, './dist'),
          },
        compress: true,
        port: 9000,
        hot: true
    },
    devtool : 'source-map',
    module: {
        rules: [
          {
            test: /\.m?js$/,
            exclude: /node_modules/,
            use: {
              loader: "babel-loader",
              options: {
                presets: ['@babel/preset-env']
              }
            }
          }
        ]
      }
}