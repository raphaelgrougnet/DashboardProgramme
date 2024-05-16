export class Guid {
    private value = this.empty;

    constructor(value?: string) {
        if (value) {
            if (Guid.isValid(value)) {
                this.value = value;
            }
        }
    }

    public static get empty() {
        return "00000000-0000-0000-0000-000000000000";
    }

    public get empty() {
        return Guid.empty;
    }

    public static newGuid() {
        return new Guid(
            "xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx".replace(/[xy]/g, c => {
                const r = Math.random() * 16 | 0;
                const v = c == "x" ? r : r & 0x3 | 0x8;
                return v.toString(16);
            })
        );
    }

    public static isValid(str: string): boolean {
        const validRegex =
            /^[0-9A-Fa-f]{8}(?:-[0-9A-Fa-f]{4}){3}-[0-9A-Fa-f]{12}$/i;
        return validRegex.test(str);
    }

    public toString() {
        return this.value;
    }

    public toJSON() {
        return this.value;
    }
}
