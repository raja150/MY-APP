

//https://github.com/nickuraltsev/ignore-case

function normalize(string) {
    return string ? (string + '').toUpperCase() : '';
}
export const isEqual = (string1, string2) => {
    return normalize(string1) === normalize(string2);
};
