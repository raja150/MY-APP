// import server from '../config/server';
import axios from 'axios';
import camelcase from 'camelcase';
import moment from 'moment';
import React from 'react';
import * as compare from 'utils/Compare';
import * as Yup from 'yup';
import { notifyError } from '../components/alert/Toast';
import { createYupSchema } from "../components/dynamicform/Controls/YupSchemaCreator";
import APIService from '../services/apiservice';
import * as dateUtil from './date';

import { EmployeeImageCell, EmployeeImageProps } from 'domain/Organization/ImageDetails/ConstImage';
const WeekDaysValues = [{ value: 1, text: 'Su' }, { value: 2, text: 'Mo' }, { value: 3, text: 'Tu' }, { value: 4, text: 'We' },
{ value: 5, text: 'Th' }, { value: 6, text: 'Fr' }, { value: 7, text: 'Sa' }];

function CellValues({ cellInfo, values, name }) {
    if (values) {
        if (cellInfo.row.original) {
            const val = values && values.filter(v => v.value === cellInfo.row.original[name]);
            return val.length > 0 ? val[0].text : '';
        }
    }
    return '';
}

function CellSwitchValues({ cellInfo, values, name }) {
    if (cellInfo.original) {
        if (cellInfo.row[name] == true) {
            return values.length > 0 ? values[0].text : '';
        } else {
            return values.length > 1 ? values[1].text : '';
        }
    } else {
        if (cellInfo.row.original[name] == true) {
            return values.length > 0 ? values[0].text : '';
        } else {
            return values.length > 1 ? values[1].text : '';
        }
    }
}

function CheckBoxCellValues({ cellInfo, values, name }) {
    if (values && cellInfo.row.original) {
        const chkValues = cellInfo.row.original[name] && cellInfo.row.original[name].toString().split(',');
        const val = chkValues ? values.filter(v => chkValues.includes(v.value.toString())) : [];
        var text = val && val.map(function (item) {
            return item.text;
        }).join(", ");
        return text;
    }
    return '';
}

function ReferenceCellValues({ cellInfo, values, name }) {
    if (values && cellInfo.row.original) {
        const chkValues = cellInfo.row.original[name] && cellInfo.row.original[name].toString().split(',');
        const val = chkValues ? values.filter(v => chkValues.includes(v.value.toString())) : [];
        var text = val && val.map(function (item) {
            return item.text;
        }).join(", ");
        return text;
    }
    return '';
}

export const getFieldsFromFieldGroup = (group) => {
    const dd = [];
    group.wizards.forEach(tab => {
        tab.fields.forEach(rows => {
            dd.push(rows);
        })
    })
    return dd;
};

export const getYepSchema = (fields) => {
    const formSchema = fields.reduce(createYupSchema, {});
    return Yup.object().shape(formSchema);

};

export const getInitialValues = (formData, fields) => {
    const iniValues = {};
    if (formData.id) {
        iniValues["id"] = formData["id"];
    }
    fields.forEach(item => {
        //When type is radio jsType is number, radio button 
        //For radio button by assign value as undefined,
        if (item.type.toLowerCase() === "radio" || item.jsType.toLowerCase() === "number") {
            iniValues[item.name] = formData[item.name] === undefined || formData[item.name] == null
                || isNaN(formData[item.name]) ? undefined : formData[item.name];
        } else if (item.jsType.toLowerCase() === "bool") {
            iniValues[item.name] = formData[item.name] || false;
        } else if (item.type === "AsyncDropdown") {
            iniValues[item.name] = formData[item.name] || ''; //Id 
            iniValues[item.asyncDwName] = item.asyncName || ''; //Async Name 
            iniValues[item.asyncName] = formData[item.asyncName] || {}; //Object

        } else {
            iniValues[item.name] = formData[item.name] || item.default || "";
        }
    });
    return iniValues;
}

export const getFromFieldData = async (fields, iniValues) => {
    const refDataURL = [];
    fields.forEach(item => {
        if (item.entityRef && item.dependent) {
            //For cascade dependence get the value when it is selected
            if (iniValues && iniValues[item.dependent]) {
                const id = iniValues[item.dependent];
                refDataURL.push({ url: `${item.refTo}/${item.dependent}/${id}`, entity: item.name })
            }
            //Get the cascade dependence value when page loaded
            else {
                //for list page 
                refDataURL.push({ url: item.refTo + '/refData', entity: item.name })
            }
        }
        //Get the entity dropdown values
        else if (item.entityRef) {
            refDataURL.push({ url: `${item.entityRefModule}/${item.entityRef}/GetList?orderby=${item.textField}`, entity: item.name })
        }
        else if (item.isReference && item.reference && item.reference.entityPropInfo) {

            if (item.reference.entityPropInfo.dataValues == null) {
                refDataURL.push({ url: `${item.reference.entityPropInfo.entityRefModule}/${item.reference.entityPropInfo.entityRef}/GetList`, entity: item.name })
            }
        }
        //Get the lookup dropdown values
        else if (item.addValues) {
            refDataURL.push({ url: `LookupValues/${item.lookupCode}`, entity: item.name })
        }
    });

    //Get reference and dropdown controls data
    // ---------------------------- 
    let urlArray = []; //Get URL list
    let urlData = []; //Push API data
    let ddData = []; //final date with field and lookup values array

    refDataURL.forEach(function (data, index) {
        urlData[urlData.length] = { entity: data.entity, data: {} };
        urlArray.push(data.url);
    });

    //promise array
    let promiseArray = urlArray.map(url => APIService.getAsync(url));

    //Calling AIP
    await axios.all(promiseArray)
        .then(function (results) {
            results.forEach(function (data, index) {
                urlData[index].data = data.data; //Hold API data in array
            });
        });

    //Map through downloaded data and make final array {fieldName, values array[{},{}]}
    urlData.forEach(function (data, index) {
        ddData[data.entity] = data;
    });
    // ----------------------------
    return ddData;
}

export const getDropDownData = async (fields) => {
    const refDataURL = [];
    fields.forEach(item => {
        //Get the entity dropdown values
        if (item.entityRef) {
            refDataURL.push({ url: `${item.entityRefModule}/${item.entityRef}/GetList?orderby=${item.textField}`, entity: item.name })
        }
    });

    //Get reference and dropdown controls data
    // ---------------------------- 
    let urlArray = []; //Get URL list
    let urlData = []; //Push API data
    let ddData = []; //final date with field and lookup values array

    refDataURL.forEach(function (data, index) {
        urlData[urlData.length] = { entity: data.entity, data: {} };
        urlArray.push(data.url);
    });

    //promise array
    let promiseArray = urlArray.map(url => APIService.getAsync(url));

    //Calling AIP
    await axios.all(promiseArray)
        .then(function (results) {
            results.forEach(function (data, index) {
                urlData[index].data = data.data; //Hold API data in array
            });
        });

    //Map through downloaded data and make final array {fieldName, values array[{},{}]}
    urlData.forEach(function (data, index) {
        ddData[data.entity] = data;
    });
    // ----------------------------
    return ddData;
}

export const getPaginateFieldData = async (fields, iniValues) => {
    const refDataURL = [];
    fields.forEach(item => {
        if (item.addValues) {
            refDataURL.push({ url: `LookupValues/${item.lookupCode}`, entity: item.name })
        }
    });

    //Get reference and dropdown controls data
    // ---------------------------- 
    let urlArray = []; //Get URL list
    let urlData = []; //Push API data
    let ddData = []; //final date with field and lookup values array

    refDataURL.forEach(function (data, index) {
        urlData[urlData.length] = { entity: data.entity, data: {} };
        urlArray.push(data.url);
    });

    //promise array
    let promiseArray = urlArray.map(url => APIService.getAsync(url));

    //Calling AIP
    await axios.all(promiseArray)
        .then(function (results) {
            results.forEach(function (data, index) {
                urlData[index].data = data.data; //Hold API data in array
            });
        });

    //Map through downloaded data and make final array {fieldName, values array[{},{}]}
    urlData.forEach(function (data, index) {
        ddData[data.entity] = data;
    });
    // ----------------------------
    return ddData;
}

export const getCaseCadeFieldData = async (fields, value) => {
    const refDataURL = [];
    fields.forEach(item => {
        if (item.refTo && item.dependent != null) {
            refDataURL.push({ url: `${item.refTo}/${item.dependent}/${value}`, entity: item.name })
        }
    });
    //Get reference and dropdown controls data
    // ---------------------------- 
    let urlArray = []; //Get URL list
    let urlData = []; //Push API data
    let ddData = []; //final date with field and lookup values array

    refDataURL.forEach(function (data, index) {
        urlData[urlData.length] = { entity: data.entity, data: {} };
        urlArray.push(data.url);
    });

    //promise array
    let promiseArray = urlArray.map(url => APIService.getAsync(url));

    //Calling AIP
    await axios.all(promiseArray)
        .then(function (results) {
            results.forEach(function (data, index) {
                urlData[index].data = data.data; //Hold API data in array
            });
        });

    //Map through downloaded data and make final array {fieldName, values array[{},{}]}
    urlData.forEach(function (data, index) {
        ddData[data.entity] = data;
    });
    // ----------------------------
    return ddData;
}

export const getTableCols = (fieldGroups, ddData) => {

    const cols = [];
    cols.push({ Header: '', accessor: 'id', show: false, });
    if (fieldGroups.module === "Organization" && fieldGroups.name === "Employee") {
        cols.push({
            ...EmployeeImageProps,
            Cell: ({ row }) => EmployeeImageCell(row.original.id)
        });
    }
    fieldGroups.wizards.forEach(fg => {
        fg.fields.forEach(f => {
            if (f.showInList === true) {
                if (f.isReference) {
                    if (f.dataValues || ddData[f.name]) {
                        cols.push({
                            Header: f.label, accessor: f.name, Cell: cellInfo => (
                                <ReferenceCellValues cellInfo={cellInfo} name={f.name}
                                    values={ddData[f.name] ? ddData[f.name].data : f.dataValues} />
                            ), id: `${f.navigation}`
                        });
                    } else {
                        cols.push({ Header: f.label, accessor: camelcase(f.propName), id: `${f.navigation}` });
                    }
                }
                else if (f.entityRef) {
                    cols.push({ Header: f.label, accessor: camelcase(f.propName), id: `${f.propName}.${f.textField}` });
                } else if (f.type.toLowerCase() === 'SwitchInput'.toLowerCase()) {
                    //For switch value is true or false, but in json it is 1 or 2
                    // "values": [{"value": 1,"text": "Active"},{"value": 2,"text": "De-active"}],
                    cols.push({
                        Header: f.label, accessor: f.name, Cell: cellInfo => (
                            <CellSwitchValues cellInfo={cellInfo} name={f.name} values={f.dataValues} />
                        )
                    });
                } else if (f.type.toLowerCase() === 'weekdays') {
                    //For switch value is true or false, but in json it is 1 or 2
                    // "values": [{"value": 1,"text": "Active"},{"value": 2,"text": "De-active"}],
                    cols.push({
                        Header: f.label, accessor: f.name, Cell: cellInfo => (
                            <CheckBoxCellValues cellInfo={cellInfo} name={f.name} values={WeekDaysValues} />
                        )
                    });
                } else if (f.type.toLowerCase() === 'radio') {
                    cols.push({
                        Header: f.label, accessor: f.name, Cell: cellInfo => (
                            <CellValues cellInfo={cellInfo} name={f.name} values={ddData[f.name] ? ddData[f.name].data : f.dataValues} />
                        )
                    });
                } else if (f.dataValues || ddData[f.name]) {
                    cols.push({
                        Header: f.label, accessor: f.name, Cell: cellInfo => (
                            <CheckBoxCellValues cellInfo={cellInfo} name={f.name} values={ddData[f.name] ? ddData[f.name].data : f.dataValues} />
                        )
                    });
                }
                else if (f.type.toLowerCase() === 'RWDatePicker'.toLowerCase() ||
                    f.type === 'timePicker'.toLowerCase() ||
                    f.type === 'datePicker'.toLowerCase()) {

                    cols.push({
                        Header: f.label,
                        accessor: f.name,
                        Cell: (props) => {
                            //props.value will contain your date
                            //you can convert your date here 
                            return <span>{dateUtil.DisplayTableDateOrTime(props.value, f.disableDate, f.enableTime)}</span>
                        }
                    });
                }
                else {
                    cols.push({ Header: f.label, accessor: f.name });
                }
            }
        })
    });

    return cols;
}

export const getTableColsFromJson = (fields, ddData) => {

    const cols = [];
    cols.push({ Header: '', accessor: 'id', show: false, });

    fields.forEach(f => {
        const type = f.type.toLowerCase();
        let col = {};
        if (type === "multiSelect".toLowerCase()) {
            col = {
                Header: f.label, accessor: f.name, Cell: cellInfo => (
                    <CheckBoxCellValues cellInfo={cellInfo} name={f.name} values={ddData[f.name] ? ddData[f.name].data : f.values} />
                )
            };
        }
        else if (type === "select") {
            if (ddData && ddData[f.name]) {
                col = {
                    Header: f.label, accessor: f.name, Cell: cellInfo => (
                        <CellValues cellInfo={cellInfo} name={f.name} values={ddData[f.name] ? ddData[f.name].data : f.values} />
                    )
                };
            } else if (f.values) {
                col = {
                    Header: f.label, accessor: f.name, Cell: cellInfo => (
                        <CellValues cellInfo={cellInfo} name={f.name} values={ddData[f.name] ? ddData[f.name].data : f.values} />
                    )
                };
            } else {
                col = { Header: f.label, accessor: f.name };
            }
        } else if (type === 'date' || type === 'dateTime'.toLowerCase()) {
            col = {
                Header: f.label,
                accessor: f.name,
                id: f.id ? f.id : f.name,
                Cell: (props) => {
                    //props.value will contain your date
                    //you can convert your date here 
                    if (type === 'date') return <span>{dateUtil.getDate(props.value, f.type)}</span>
                    else
                        return <span>{dateUtil.getFormattedDateTime(props.value, f.type)}</span>
                }
            };
        }
        else if (type === 'switch') {
            //For switch value is true or false, but in json it is 1 or 2
            // "values": [{"value": 1,"text": "Active"},{"value": 2,"text": "De-active"}],
            col = {
                Header: f.label, accessor: f.name, Cell: cellInfo => (
                    <CellSwitchValues cellInfo={cellInfo} name={f.name} values={ddData[f.name] ? ddData[f.name].data : f.values} />
                )
            };
        }
        else if (type === 'custom') {
            col = {
                Header: f.label, accessor: f.name, Cell: f.Cell, disableSortBy: f.disableSorting ? true : false,
            };
        }
        else {
            col = { Header: f.label, accessor: f.name, id: f.id ? f.id : f.name };
        }
        if (col) {
            if (f.width) {
                col = { ...col, width: f.width }
            }
            cols.push(col);
        }
    });

    return cols;
}

export const setFormikError = (error, setFieldError) => {
    const data = error.response && error.response.data ? error.response.data : undefined;
    if (error.response && error.response.status === 404) {
        notifyError("Sorry an error occurred.");
        return;
    } else if (error.response && error.response.status === 403) {
        notifyError("Sorry! you do not have permission.");
        return;
    }

    if (data && data.errors) {
        //For framework errors
        for (var x in data.errors) {
            setFieldError(x, error.response.data.errors[x]);
        }
    } else if (data && Array.isArray(data.messages)) {
        //When single error and it is general error 
        //For model state errors
        let erMsg;
        data.messages.forEach((item) => {
            if (item.field) {
                setFieldError(camelcase(item.field), item.description);
            } else {
                //0 for runtime exceptions messages
                if (item.feedbackType > 0) {
                    erMsg = item.description;
                } else {
                    erMsg = "Sorry, an error occurred while processing your request.";
                }
            }
        });
        if (erMsg) {
            notifyError(erMsg);
        }
    } else if (Array.isArray(data)) {
        //For model state errors
        data.forEach((item) => {
            setFieldError(camelcase(item.name), item.message);
        })
    } else {
        notifyError("Sorry, an error occurred while processing your request.");
    }
}

export const displayFormikError = (error, actions) => {
    setFormikError(error, actions.setFieldError);
}
export const displayErrors = (error) => {
    if (error.response && error.response.status === 403) {
        notifyError("Sorry! you do not have permission.");
        return;
    }
    if (error.response && error.response.status === 404) {
        notifyError("Sorry, an error occurred while processing your request.");
        return;
    }
    const data = error.response && error.response.data ? error.response.data : undefined;
    //When single error and it is general error 
    //For model state errors
    let erMsg;
    data && data.messages && data.messages.forEach((item) => {
        if (item.feedbackType > 0) {
            erMsg = item.description;
        } else {
            erMsg = "Sorry, an error occurred while processing your request.";
        }
    });
    if (erMsg) {
        notifyError(erMsg);
    }
    else if (data && data.title) {
        notifyError(data.title);
    }
}
export const displayAPIError = (error) => {
    const data = error.response && error.response.data ? error.response.data : undefined;

    if (data && Array.isArray(data.messages)) {
        let erMsg;
        data.messages.forEach((item) => {
            erMsg = item.description;
        });
        if (erMsg) {
            notifyError(erMsg);
        }
    } else {
        notifyError("Sorry, an error occurred while processing your request.");
    }
}

//Get text from const array
//array format is [{value:1, text:''}]
export const GetTextFromArray = (array, value) => {
    const obj = array.find(e => compare.isEqual(e.value, value));
    return obj ? obj.text : '';
}
export const YEARS = () => {
    let years = []
    for (var i = moment().format('YYYY'); i >= parseInt(moment().format('YYYY')) - 2; i--) {
        years.push({
            id: parseInt(i),
            text: i
        });
    }
    return years
}
export const MONTHS = [
    { id: 1, text: 'January' },
    { id: 2, text: 'February' },
    { id: 3, text: 'March' },
    { id: 4, text: 'April' },
    { id: 5, text: 'May' },
    { id: 6, text: 'June' },
    { id: 7, text: 'July' },
    { id: 8, text: 'August' },
    { id: 9, text: 'September' },
    { id: 10, text: 'October' },
    { id: 11, text: 'November' },
    { id: 12, text: 'December' },

]

export const GetFileExtension = (path) => {
    var basename = path.split(/[\\/]/).pop(),
        pos = basename.lastIndexOf(".");

    if (basename === "" || pos < 1)
        return "";

    return basename.slice(pos + 1);
}

//to convert SerializeObject into CamelCase
export const getCamelCaseObject = obj =>
    Object.keys(obj).reduce((acc, k) => {
        acc[k.toLowerCase()] = obj[k];
        return acc;
    }, {});