
// is convert string into number/decimal
export const RoundDeceimalToNumber = (value) => {
    if (value) {
        return Math.round(value, 0);
    }
    return 0;
}

export const RoundOffDiff = (value) => {
    if (value) {
        const r = RoundDeceimalToNumber(value);
        const f = RoundOff(value);
        return RoundOff(r > f ? (r - f) : (f - r));
    }
    return 0;
}
//Differnce between RoundOffDiff an this is return with negative symbal
export const RoundOffDiffWithSign = (value) => {
    if (value) {
        const r = RoundDeceimalToNumber(value);
        const f = RoundOff(value);
        return RoundOff(r - f);
    }
    return 0;
}

export const RoundOff = (value) => {
    //Math.round((num  Number.EPSILON) * 100) / 100
    //1.005 should be convert as 1.01, but in parseFloat is convert as 1.00 
    if (value) {
        return (Math.round((value + Number.EPSILON) * 100) / 100);
    }
    return 0;
}

export const MoneyFormat = (value) => {
    if (value) {
        return parseFloat(value)
    }
    return parseFloat(0);
}
export const MoneyFormatWithDecimal = (value) => {
    if (value) {
        return "₹ " + parseFloat(value).toFixed(2)
    }
    return "₹ " + parseFloat(0).toFixed(2);
}

export const MoneyFormatWithOutDecimal = (value) => {
    if (value) {
        return "₹ " + parseFloat(value)
    }
    return "₹ " + parseFloat(0);
}
export const SumOfValues = (value1, value2) => {
    return parseFloat(value1) + parseFloat(value2);
}
