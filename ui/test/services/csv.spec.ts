import { describe, test, expect } from "@jest/globals";
import { fromCSV } from "../../src/services/CSVService";

describe("CSVService", () => {
    describe("fromCSV", () => {
        test("Load malformed data", () => {
            expect(fromCSV("")).toHaveLength(0);
            expect(fromCSV("STEPS;RAW\n0;1\n1;b\n")).toStrictEqual([ [ 0, 1 ] ]);
            expect(fromCSV("X;RAW\n0;1\n1;2\n")).toHaveLength(0);
        });

        test("Load normal data", () => {
            expect(fromCSV("STEPS;RAW")).toHaveLength(0);
            expect(fromCSV("STEPS;RAW\n")).toHaveLength(0);

            expect(fromCSV("STEPS;RAW\n0;1\n")).toStrictEqual([ [ 0, 1 ] ]);
            expect(fromCSV("STEPS;RAW\n0;1\n2;3\n")).toStrictEqual([ [ 0, 1 ], [ 2, 3 ] ]);

            expect(fromCSV("STEPS;RAW;EXTRA\n0;1;B\n2;3;C\n")).toStrictEqual([ [ 0, 1 ], [ 2, 3 ] ]);
        });

        test("Load swapped headers", () => {
            expect(fromCSV("RAW;A;STEPS\n3;2;1\n6;5;4\n")).toStrictEqual([ [ 1, 3 ], [ 4, 6 ] ]);
        });
    });
});
