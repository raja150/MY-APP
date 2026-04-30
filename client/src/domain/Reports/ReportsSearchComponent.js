import React, { useState, useEffect } from 'react';
import { Col, Row, Button } from 'reactstrap';
import { Input, Radio, RWDatePicker, RWDropdownList } from 'components/dynamicform/Controls';
import ReportAsyncSearch from 'components/dynamicform/Controls/ReportAsyncSeach';
import MothAndYearPicker from 'components/dynamicform/Controls/MothAndYearPicker';
import * as _ from 'lodash';
import * as compare from 'utils/Compare';
import queryString from 'query-string';
import APIService from '../../services/apiservice';
import axios from 'axios';
import { useHistory } from 'react-router-dom';
import { displayErrors } from 'utils/form';
import ReportTable from './ReportsTable';
import Loading from 'components/Loader';
import { notifyError } from 'components/alert/Toast';

export default function ReportSearchComponent(props) {
    const fieldMap = {
        Dropdown: RWDropdownList,
        DatePicker: RWDatePicker,
        Input: Input,
        Radio: Radio,
        AsyncDropdown: ReportAsyncSearch,
        MonthYearPicker: MothAndYearPicker
    };
    const [stateNew, setStateNew] = useState({ reportsData: [], summaryData: [] });
    let queryProps = { ...queryString.parse(props.location.search) };
    const [isSearching, setIsSearching] = useState(false); 
    const [ddValues, setDdValues] = useState([]); 

    // useEffect(() => {
    //     //to clear table data when changing the report and module
    //     setStateNew({ reportsData: [], summaryData: [] })
    // }, [props.reportName])

    useEffect(() => {
        const fetchData = async () => {
            let json = {}
            props.filterValues['module'] = props.module
            props.filterValues['report'] = props.reportName
            let dd = await getReportFieldData(props.json);

            if (!_.isEmpty(queryProps)) {
                //Updating filter data when toggle between tabs 
                //And arrange data in sorting order
                handleDataInitially(json, props.module, props.reportName)
                //Getting Report data 
                //when toggle between tabs from API
                if (props.reportName == queryProps['report'])
                    await getTableData(queryProps)
            }
            setDdValues(dd)
            props.setFilterValues(props.filterValues);
            setStateNew({ reportsData: [], summaryData: [] })
        }
        fetchData();
    }, [props.reportName])

    //API Calls
    const getTableData = async (queryValue) => {
        let reportsData = [], summaryTableData = []
        setIsSearching(true);
        await APIService.getAsync(`${props.json.url}?` + queryString.stringify(queryValue)).then(res => {
            reportsData = res.data;

        }).catch(err => {
            displayErrors(err)
        })
        if (props.json.summaryTable && props.json.summaryTable.length > 0 && queryValue.fromDate && queryValue.toDate) {
            await APIService.getAsync(`${props.json.summaryTableUrl}?` + queryString.stringify(queryValue)).then(res => {
                summaryTableData = res.data;
            }).catch(err => {
                notifyError(err.message)
            })
        }
        setStateNew({ reportsData: reportsData, summaryData: summaryTableData });
        setIsSearching(false);
    }

    //Adding all Dropdowns data from API
    const getReportFieldData = async (fields) => {
        const refDataURL = [];
        fields.filters.forEach(item => {
            if (item.url && item.field === "Dropdown") {
                refDataURL.push({ entity: item.name, url: item.url })
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

    const handleRenderingData = (json, selected, module, report, fieldName) => {
        //const initValues = {};
        props.filterValues['module'] = module;
        props.filterValues['report'] = report;
        json && json.filters.length > 0 && json.filters.sort((a, b) => a.orderBy - b.orderBy)
            .forEach((field) => {
                switch (field.field) {
                    case "AsyncDropdown":
                        let key = field.name + 'Key';
                        let sel = {}
                        //Checking selected object is exist or not
                        // in the searchUrl;
                        if (selected) {
                            //on OnChange of SearchComponent we will get selected name
                            if (field.name == fieldName) {
                                sel[field.textField] = selected
                                field['selected'] = sel

                            } else {
                                sel[field.textField] = field['selected'] ? field['selected'][field.textField]
                                    : queryProps[key] ? queryProps[key] : ''
                            }
                        } else if (selected == '') {
                            //Re-select or changing the selected one
                            if (field.name == fieldName) {
                                field['selected'] = queryString.parse(selected)
                            } else {
                                //setting previous selected data when we have multple Search componets
                                sel[field.textField] = field['selected'] ? field['selected'][field.textField]
                                    : queryProps[key] ? queryProps[key] : ''
                                field['selected'] = sel
                            }
                        }
                        if (queryProps[field.name]) {
                            props.filterValues[field.name] = queryProps[field.name];
                        }
                        if (queryProps[key]) {
                            props.filterValues[key] = queryProps[key];
                        }
                        break;
                    default:
                        props.filterValues[field.name] = props.filterValues[field.name] ?
                            props.filterValues[field.name] : queryProps[field.name] ? queryProps[field.name] : ''

                        break;
                }
            });
    }

    const handleFilterChange = async (name, value) => {
        let vv = {};
        if (name === "lobId") {
            const q = value ? `LmsReport/FunctionalArea?lobId=${value}` : 'LmsReport/FunctionalArea';
            await APIService.getAsync(q).then(res => {
                ddValues['functionalAreaId'].data = res.data;
                ddValues['designationId'].data = [];
                vv.functionalAreaId = '';
                vv.designationId = '';

            }).catch(err => {
                notifyError(err.message)
            })

        }
        if (name === "functionalAreaId") {
            const q = value ? `LmsReport/Designations?functionalAreaId=${value}&lobId=${props.filterValues.lobId}` : 'LmsReport/Designations';
            await APIService.getAsync(q).then(res => {
                ddValues['designationId'].data = res.data;
                vv.designationId = '';
            }).catch(err => {
                notifyError(err.message)
            })

        }
        if (name === "moduleId") {
            const q = value ? `LmsReport/Pages?moduleId=${value}` : 'LmsReport/Pages';
            await APIService.getAsync(q).then(res => {
                ddValues['pageId'].data = res.data;
                vv.pageId = '';
            }).catch(err => {
                notifyError(err.message)
            })

        }

        let labelKey = name + "Key"
        if (!_.isEmpty(queryProps) && queryProps[labelKey]) {
            props.setFilterValues({ ...props.filterValues, ...vv, [name]: value, [labelKey]: labelKey });
        } else {
            props.setFilterValues({ ...props.filterValues, ...vv, [name]: value, });
        }

    }

    const handleAsyncChange = (name, value, selected) => {
        //setting object to display selected item for asyncSearch component
        //When toggle between tabs to render initially
        let labelKey = name + 'Key'
        // if (!_.isEmpty(queryProps)) {
        handleRenderingData(props.json, selected, props.module, props.reportName, name)
        // }
        props.setFilterValues({ ...props.filterValues, [name]: value, [labelKey]: selected });
    }

    const handleSearch = async () => {
        // setIsSearching(true);
        setStateNew({ reportsData: [] });
        const qry = { ...props.filterValues }
        //Adding filters details to search parameters
        // Object.keys(props.filterValues).forEach(function (key) {
        //     if (parameters.has(key)) {
        //         parameters.set(key, props.filterValues[key]);
        //     } else {
        //         parameters.append(key, props.filterValues[key]);
        //     }
        // });

        // history.push({ search: parameters.toString() })
        await getTableData(qry);
        // setIsSearching(false);
    }

    const handleDataInitially = (json, module, report,) => {
        //const initValues = {};
        props.filterValues['module'] = module;
        props.filterValues['report'] = report;
        props.json && props.json.filters.length > 0 && props.json.filters.sort((a, b) => a.orderBy - b.orderBy)
            .forEach((field) => {
                switch (field.field) {
                    case "AsyncDropdown":
                        let key = field.name + 'Key';
                        let sel = {}
                        //When page in initially rendering,like when toggle between tabs
                        sel[field.textField] = queryProps[key] ? queryProps[key] : ''
                        sel[field.valueField] = queryProps[field.name] ? queryProps[field.name] : ''
                        props.filterValues[key] = queryProps[key] ? queryProps[key] : '';
                        field['selected'] = sel
                        props.filterValues[field.name] = queryProps[field.name] ? queryProps[field.name] : '';
                        break;
                    default:
                        //For remaining filters setting the selected values
                        props.filterValues[field.name] = queryProps[field.name] ? queryProps[field.name] : ''
                        break;
                }
            });
    }

    const getComponents = (field, key) => {

        const Component = fieldMap[field.field];
        return (
            Component ?
                <Col key={`cp-${key}`} xs='3'>
                    <Component
                        key={field.name}
                        name={field.name}
                        label={field.label}
                        value={props.filterValues[field.name] || field.default}
                        values={ddValues && ddValues[field.name] ? ddValues[field.name].data :
                            (_.isEmpty(field.data) ? [] : field.data)}
                        valueField={field.valueField || 'value'}
                        textField={field.textField || 'text'}
                        showDate={compare.isEqual(field.field, "DatePicker") ? true : compare.isEqual(field.field, "MonthYearPicker") ? 1 : false}
                        showTime={compare.isEqual(field.field, 'TimePicker') ? true : compare.isEqual(field.field, "MonthYearPicker") ? 0 : false}
                        handlevaluechange={handleFilterChange}
                        url={field.url ? field.url : ''}
                        handleAsyncChange={handleAsyncChange}
                        selected={!_.isEmpty(field.selected) ? field.selected : {}}
                        extension={field.dependingOn && props.filterValues[field.dependingOn]}
                        editFormat={field.editFormat ? field.editFormat : ''}
                        format={field.format ? field.format : ''}
                        views={field.views ? field.views : ''}
                    />
                </Col> : ''
        )
    }

    return (
        <>
            <Row>
                {props.filters.length > 0
                    && props.filters.sort((a, b) => a.orderBy - b.orderBy).map((field, i) => {
                        return getComponents(field, i);
                    })}
                <Col xs='1'>
                    <Button className="mb-2 mr-2 mt-3 btn-icon btn-success1" key='buttonSave'
                        color='success' type="button" disabled={isSearching} onClick={() => handleSearch()} name="search"  >{isSearching ? 'Please wait' : 'Search'} </Button>
                </Col>
            </Row>
            {isSearching ?
                <Row>
                    <Col xs='12' ><Loading /></Col>
                </Row>
                :
                <ReportTable columns={props.json.columns} data={stateNew.reportsData}
                    popUpfilters={props.json.popUpfilters}
                    summaryColumns={props.json.summaryTable} summaryData={stateNew.summaryData} />
            }
        </>
    )
}

