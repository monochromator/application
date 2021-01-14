module.exports = {
    root: true,
    env: {
        node: true
    },
    extends: [
        "plugin:vue/essential",
        "eslint:recommended",
        "@vue/standard",
        "@vue/typescript/recommended"
    ],
    parserOptions: {
        ecmaVersion: 2020
    },
    rules: {
        "no-console": process.env.NODE_ENV === "production" ? "error" : "off",
        "no-debugger": process.env.NODE_ENV === "production" ? "error" : "off",
        "vue/script-indent": [ "error", 4, { baseIndent: 1 } ],
        "array-bracket-spacing": [ "error", "always" ],
        "object-curly-spacing": [ "error", "always" ],
        "linebreak-style": [ "error", "unix" ],
        "space-before-function-paren": [ "error", "never" ],
        indent: [ "error", 4, { SwitchCase: 1 } ],
        quotes: [ "error", "double" ],
        semi: [ "error", "always" ]
    },
    overrides: [
        {
            files: [ "*.vue" ],
            rules: { indent: "off" }
        }
    ]
};
