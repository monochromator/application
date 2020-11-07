module.exports = {
    moduleFileExtensions: [
        "js",
        "json",
        "ts"
    ],
    transform: {
        // process TypeScript files
        "^.+\\.ts$": "ts-jest"
    },
    // support the same @ -> src alias mapping in source code
    moduleNameMapper: {
        "^@/(.*)$": "<rootDir>/src/$1"
    }
};
