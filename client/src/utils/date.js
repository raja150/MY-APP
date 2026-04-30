const moment = require('moment');

//https://momentjs.com/docs/#/displaying/format/
export const getFormattedDateTime = (value, componentType) => {
    if (value == null) {
        return ""
    }
    if (componentType.toLowerCase() === 'datetime') {
        return moment(value).format(moment.HTML5_FMT.DATETIME_LOCAL).replace('T', ' ');
    }
    else if (componentType === 'timepicker') {
        return moment(value).format(moment.HTML5_FMT.TIME);
    }
    else if (componentType === 'datepicker') {
        return moment(value).format("MM/DD/YYYY");
    }
};

export const getShortDate = (value) => {
    if (value) {
        return moment(value).format("DD MMM, ddd");
    }
}

export const getDayMonth = (value) => {
    if (value) {
        return moment(value).format("DD MMM");
    }
}
export const getMonth = (value) => {
    if (value) {
        return moment(value).format("MM")
    }
}
export const getDate = (value) => {
    if (value) {
        return getFormattedDateTime(value, "datepicker");
    }
}

export const getTodayDate = () => {
    return moment().format(moment.HTML5_FMT.DATE);
}

export const getRequiredDate = (value) => {
    return moment(value).format(moment.HTML5_FMT.DATE);
}
export const lastDayOfMOnth = (value) => {
    return moment(value).endOf('month');
}

export const getToday = () => {
    return moment().toDate();
}

export const getExpireDate = (value) => {
    if (value) {
        return moment(value).format('MM/YY');
    }
    return "";
}

export const DisplayDateTime = (value) => {
    if (value) {
        return moment(value).format(moment.HTML5_FMT.DATETIME_LOCAL).replace('T', ' ');
    }
    return "";
}

export const DisplayTableDateOrTime = (value, disableDate = true, enableTime = false) => {
    if (disableDate && enableTime) {
        return getFormattedDateTime(value, 'timepicker');
    }
    else if (disableDate && enableTime === false) {
        return getFormattedDateTime(value, 'datepicker');
    }
    else {
        return getFormattedDateTime(value, 'datetime');
    }
    return "";
}

export const DisplayMinDate = (num, value) => {
    return moment().add(num, value)
}
export const getAgeWithDob = (dob) => {

    let age = Math.floor((moment().format('YYYY') - moment(dob).format('YYYY')))
    // if (age == 0) {
    //     age = 'Years :' + 0 + ',' + 'month(s) :' + Math.floor((moment().format('MM') - moment(dob).format('MM'))) + ',' + 'Days :' + Math.floor((moment().format('DD') - moment(dob).format('DD')))
    //     setFieldValue('age', age)
    // } else {
    if (Math.floor(moment().format('MM')) < Math.floor(moment(dob).format('MM')) ||
        (Math.floor((moment().format('MM')) === Math.floor(moment(dob).format('MM')) && Math.floor(moment().format('DD')) < Math.floor(moment(dob).format('DD'))))) {
        return age - 1
    } else {
        return age
    }
    // }
}
export const getDobWithAge = (age) => {
    let year = Math.floor((moment().format('YYYY') - age))
    let month = Math.floor(moment().format('MM'))
    let format = year + '-' + month + '-' + 1
    let dob = moment(format).format('YYYY-MM-DD')
    return dob;
}
export const getDaysDifference = (fromDate, toDate, fromHalfDisplay, toHalfDisplay) => {
    const date1 = new Date(fromDate);
    const date2 = new Date(toDate);

    // One day in milliseconds
    const oneDay = 1000 * 60 * 60 * 24;

    // Calculating the time difference between two dates
    const diffInTime = date2.getTime() - date1.getTime();

    // Calculating the no. of days between two dates
    var diffInDays = Math.round(diffInTime / oneDay);

    //if fromHalf is true then Adding The HalfDay
    if (fromHalfDisplay == 1) {
        diffInDays = diffInDays - 0.5;
    }
    if (toHalfDisplay == 1) {
        diffInDays = diffInDays - 0.5;
    }

    return diffInDays + 1;
}

//Calculating hours and minutes from total minutes
export const toHoursAndMinutes = (totalMinutes) => {
    if (totalMinutes == null) {
        return "00:00"
    }
    const hours = Math.floor(totalMinutes / 60);
    const minutes = totalMinutes % 60;
    //to display 00:00 format
    var HrsAndMins = (hours >= 10 ? hours : "0" + hours) + ":" + (minutes >= 10 ? minutes : "0" + minutes)
    return HrsAndMins
}
export const getTime = (date)=>{
    //moment(currentTime).format("hh:mm"));
    return moment(date).format("hh:mm A");
}