import React from 'react';
//https://stackoverflow.com/questions/55556221/how-do-you-format-a-number-to-currency-when-using-react-native-expo
const defaultOptions = {
    significantDigits: 2,
    thousandsSeparator: ',',
    decimalSeparator: '.',
    symbol: '₹'
}
export const WithOutSymbol = ({ value, options }) => {
   
    if (typeof value !== 'number') value = 0.0
    options = { ...defaultOptions, ...options }
    value = value.toFixed(options.significantDigits) 
    const [currency] = value.split('.')
    return `${currency.replace(
        /\B(?=(?:(\d\d)+(\d)(?!\d))+(?!\d))/g,
        options.thousandsSeparator
    )}`
}
const MoneyFormat = ({ value, options }) => {
    if (typeof value !== 'number') value = 0.0
    options = { ...defaultOptions, ...options }
    value = value.toFixed(options.significantDigits)

    const [currency] = value.split('.')
    return `${options.symbol} ${currency.replace(
        /\B(?=(?:(\d\d)+(\d)(?!\d))+(?!\d))/g,
        options.thousandsSeparator
    )}`
}

export const MoneyFormatWithDecimal = ({ value, options }) => {
    if (typeof value !== 'number') value = 0.0
    options = { ...defaultOptions, ...options }
    value = value.toFixed(options.significantDigits)

    const [currency, decimal] = value.split('.')
    return `${options.symbol} ${currency.replace(
        /\B(?=(?:(\d\d)+(\d)(?!\d))+(?!\d))/g,
        options.thousandsSeparator
    )}${options.decimalSeparator}${decimal}`
}

export default MoneyFormat;