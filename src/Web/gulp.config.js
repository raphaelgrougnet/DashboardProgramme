/* config ———————————————————————————————————————————————————————————————————*/
const config = {
    browsersync: {
        localhost: undefined,
        port: 8080,
        notify: false,
        sync: true,
        open: false,
    },

    sass: {
        src: ["./Sass/*.{scss,sass}", "./Sass/*/*.{scss,sass}"],
        dest: "./wwwroot/css",
        options: {indentedSyntax: false},
        autoprefixer: {
            browsers: ["last 6 versions", "ie 10", "iOS 7-8"],
        },
    },

    js: {
        src: "./scripts/index.js",
        dest: "./wwwroot/js",
        eslint: {fix: true},
        babel: {
            presets: ["es2015-rollup"],
        },
    },

    svg: {
        src: "./wwwroot/icons/*.svg",
        dest: "./wwwroot/assets",
    },

    views: {
        src: "./Views/**/*.html",
        dest: "./",
    },

    watch: {
        sass: "Sass/**/*.{scss,sass}",
        js: "scripts/**/*.js",
        views: "./Views/**/*.html",
    },
};

/* helpers ——————————————————————————————————————————————————————————————————*/
const notifier = require('node-notifier');
const symbols = require('log-symbols');
const chalk = require('chalk');

module.exports = {
    get config() {
        config.sass.dest = config.sass.dest.replace(/\/$/, '');
        config.js.dest = config.js.dest.replace(/\/$/, '');
        config.views.dest = config.views.dest.replace(/\/$/, '');
        return config
    },

    get bsConfig() {
        let conf = {
            port: config.browsersync.port,
            ui: {port: config.browsersync.port + 1},
            notify: config.browsersync.notify,
            proxy: config.browsersync.localhost,
            server: !config.browsersync.localhost ? './' : false,
            open: config.browsersync.open
        };

        if (!config.browsersync.sync) conf.ghostMode = false;
        return conf
    },

    sassReporter(e) {
        let title = `${e.relativePath}:${e.line}`;
        let message = e.messageOriginal.replace(/\s{4}/, '');
        let count = chalk.bold(chalk.red(`${symbols.error} 1 problem (1 error, 0 warnings)`));
        console.log(chalk.underline(title), '\n ', message, `\n\n${count}`, '\n');
        notifier.notify({title, message});
        this.emit('end')
    },

    jsReporter(e) {
        let message = `${e.message}`;
        let count = chalk.bold(chalk.red(`${symbols.error} 1 problem (1 error, 0 warnings)`));
        console.log(message, `\n\n${count}`, '\n');

        notifier.notify({message});
        this.emit('end')
    }
};