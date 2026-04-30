var CryptoJS = require("crypto-js");
const key="123j1k23j1k23j1k23j1i2i1p23i12n3123asdfasdf";
export const encrypt = (text) =>{
    return CryptoJS.AES.encrypt(text+'', key);
}

export const decrypt = (text) =>{
    return CryptoJS.enc.Latin1.stringify(CryptoJS.AES.decrypt(text, key));
}