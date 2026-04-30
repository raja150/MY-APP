import { RWDatePicker, RWDropdownList } from 'components/dynamicform/Controls'
import Loading from 'components/Loader'
import EmployeeSearch from 'domain/EmployeeSearch'
import { Form, Formik } from 'formik'
import queryString from 'query-string'
import React, { Fragment, useEffect, useState } from 'react'
import PageHeader from 'Layout/AppMain/PageHeader';
import { Button, Card, CardBody, Col, Label, Row } from 'reactstrap'
import * as crypto from 'utils/Crypto'
import * as Yup from 'yup'
import * as _ from 'lodash'
import { PERFORMANCE_TYPE } from 'Site_constants'
import { SingleDatePicker } from 'react-dates'
import "react-dates/initialize";
import "react-dates/lib/css/_datepicker.css";
import classNames from 'classnames'
import moment from 'moment'
import PerformanceService from 'services/Org/Performance'
import { notifySaved, notifyError } from 'components/alert/Toast'
import { PERFORMANCE } from '../Users/Navigation'


export default function Performance(props) {
    const currentMoment = moment();
    const parsed = queryString.parse(props.location.search);
    const rid = parsed.r ? crypto.decrypt(parsed.r) : null;

    const [state, setState] = useState({
        isLoading: false, entityData: {}, focused: false, selectedYear: currentMoment.year(),
        selectedWorkWeek: currentMoment.isoWeek(), selectedMonth: '', selectedDate: ''
    })
    const [selEmp, setSelEmp] = useState({})
    const [activeWeek, setActiveWeek] = useState([])
    useEffect(() => {
        let entityData = {}, s = {}
        setState({ ...state, isLoading: true })
        const fetchData = async () => {
            if (rid) {
                await PerformanceService.getById(rid).then((res) => {
                    entityData = res.data
                    setSelEmp(res.data)
                    s['day'] = moment().year(new Date(entityData.performedDate).getFullYear()).isoWeek(entityData.weekNumber);
                    setActiveWeek(calculateActiveWeek(s.day))
                    renderCalendarDay(s)
                })
            }
            setState({
                ...state, isLoading: false, entityData: entityData,
                selectedYear: !_.isEmpty(entityData) ? new Date(entityData.performedDate).getFullYear() : currentMoment.year(),
                selectedWorkWeek: !_.isEmpty(entityData) ? entityData.weekNumber : currentMoment.isoWeek()
            })
        }
        fetchData()
    }, [])

    const validationSchema = Yup.object().shape({
        employeeId: Yup.string().required('Employee name is required!'),
        performanceType: Yup.number().required('Performance type is required!'),
        performedDate: Yup.string().nullable().when('performanceType', { is: val => (val != 2), then: Yup.string().required('Performed Date is required') }),
    })

    let formValues = {};
    if (rid) {
        formValues = state.entityData;
    }
    else {
        formValues = {
            employeeId: '',
            performanceType: '',
            performedDate: '',
        }
    }

    const isDayHighlighted = date => {
        let isHighlighted = false;
        activeWeek.forEach(hoveredDay => {
            const isDayOfMonthMatch = hoveredDay.date() === date.date();
            const isMonthMatch = hoveredDay.month() === date.month();
            const isYearMatch = hoveredDay.year() === date.year();
            if (isDayOfMonthMatch && isMonthMatch && isYearMatch) {
                isHighlighted = true;
            }
        });
        return isHighlighted;
    };

    const calculateActiveWeek = (date) => {
        if (date) {
            const mon = date.clone().startOf("isoWeek");
            const tue = mon.clone().add(1, "d");
            const wed = mon.clone().add(2, "d");
            const thu = mon.clone().add(3, "d");
            const fri = mon.clone().add(4, "d");
            const sat = mon.clone().add(5, "d");
            const sun = mon.clone().add(6, "d");
            return [mon, tue, wed, thu, fri, sat, sun];
        }
        return []
    };

    const onDateChange = async (date) => {
        //setDisplayButtons(true);
        //getWeekData(date);//Calling
        // let ary = calculateActiveWeek(date) //Getting week
        // let displayDates = [];
        // let totalInfo = [];
        // ary.forEach((e) => {
        //     displayDates.push({
        //         day: moment(e).format('ddd, DD MMM')//Pushing day and dates
        //     })
        // });
        //Header dates
        //setDisplayDates(displayDates);
        // let lineItem = NEW_TASK_LINE_ITEM;
        // lineItem.taskTexts = []
        // lineItem.days = [];

        //Setting initial data
        // ary.forEach((e) => {
        //     var date = moment(e).format('YYYY-MM-DD')
        //     lineItem.days.push({ date: date, hours: 0, minutes: 0, totalMinutes: 0 })
        // });

        //To get initial totalInfo
        // ary.forEach((e) => {
        //     totalInfo.push({ date: moment(e).format('YYYY-MM-DD'), hours: 0 });
        // });

        //updating initial values
        //let data = [];
        //data.push(lineItem);

        // setInitialValues({
        //     ...initialValues, tasks: data, totalInfo: totalInfo, projects: entityData.projects,
        //     activities: entityData.activities, total: 0
        // });
        // setDisplayDates(displayDates);
        const selectedYear = date.year();
        const selectedWorkWeek = date.isoWeek();
        const selectedMonth = date.month();
        const selectedDate = moment(new Date(date)).format('MM-DD-YYYY');
        //updating state
        await setState({
            ...state, selectedYear: selectedYear, selectedWorkWeek: selectedWorkWeek,
            selectedDate: selectedDate, selectedMonth: selectedMonth, focused: false
        });
    };
    const onDateHovered = (date) => {
        setActiveWeek(calculateActiveWeek(date))
    }

    const renderCalendarDay = (date) => {
        const dayClasses = classNames(
            "CalendarDay",
            "CalendarDay__default",
            "CalendarDay_1",
            "CalendarDay__default_2"
        );
        let style = {
            width: "39px",
            height: "38px"
        };
        if (date.day) {
            const dayOfMonth = date.day.date();
            const isHighlighted = isDayHighlighted(date.day);
            let style = {
                width: "39px",
                height: "38px",
                backgroundColor: isHighlighted ? "#42a5f5" : "white",
                color: isHighlighted ? "white" : "black"
            };
            return (
                <td
                    style={style}
                    className={dayClasses}
                    onClick={() => onDateChange(date.day)}
                    onMouseEnter={() => onDateHovered(date.day)}
                >
                    {dayOfMonth}
                </td>
            );
        } else {
            return <td style={style} className={dayClasses} />;
        }
    };
    //open calender
    const openPicker = () => {
        if (!state.focused) {
            setState({ ...state, focused: true });
        }
    };

    // To Close the Calender
    const onClose = () => setState({ ...state, focused: false })

    const handleSubmit = async (values, actions) => {
        const data = values;
        data.performedDate = state.selectedDate ? state.selectedDate : values.performedDate
        data.weekNumber = state.selectedWorkWeek
        PerformanceService.postAsync(data).then((res) => {
            notifySaved();
            props.history.push(PERFORMANCE);
        }).catch((err) => {
            notifyError(err)
        })
    }

    return (
        <Fragment>
            <PageHeader title="Star Employee" />
            <Card>
                <CardBody>
                    {state.isLoading ? <Loading /> :
                        <Formik
                            initialValues={formValues}
                            validationSchema={validationSchema}
                            onSubmit={(values, actions) => handleSubmit(values, actions)} >
                            {({ values, errors, touched, setFieldValue }) => {

                                const handleValueChange = async (name, value) => {
                                    setFieldValue(name, value)
                                }
                                return (<Form>
                                    <Row>
                                        <Col md='4'>
                                            <Label htmlFor='employee'>Employee Name</Label>
                                            <EmployeeSearch name={'employeeNo'} selEmp={selEmp} setSelEmp={setSelEmp}
                                                handleValueChange={handleValueChange} />
                                            <p style={{ color: 'red' }}>{errors.employeeId}</p>
                                        </Col>
                                        <Col md='4'>
                                            <RWDropdownList {...{
                                                name: 'performanceType', label: 'Performance Type', textField: 'text', valueField: 'value',
                                                value: values['performanceType'], values: PERFORMANCE_TYPE, error: errors['performanceType'], touched: touched['performanceType']
                                            }} handlevaluechange={handleValueChange} />
                                        </Col>
                                        {values.performanceType == 2 ?
                                            <Col md='4'>
                                                <Label>Select Week&nbsp;</Label>
                                                <SingleDatePicker
                                                    small={true}
                                                    block={true}
                                                    // showDefaultInputIcon ={true}
                                                    // inputIconPosition={}
                                                    focused={state.focused}
                                                    date={moment().year(state.selectedYear).isoWeek(state.selectedWorkWeek)}
                                                    onFocusChange={() => openPicker()}
                                                    id="single_date_picker"
                                                    numberOfMonths={1}
                                                    hideKeyboardShortcutsPanel={true}
                                                    isDayBlocked={() => false}
                                                    isOutsideRange={() => false}
                                                    firstDayOfWeek={1}
                                                    renderCalendarDay={(e) => renderCalendarDay(e)}
                                                    onClose={() => onClose()}
                                                />
                                            </Col>
                                            :
                                            <Col md='4'>
                                                <RWDatePicker {...{
                                                    name: 'performedDate', label: 'Performed Month', showDate: true, showTime: false,
                                                    value: values['performedDate'],
                                                    format: "MM/YY",
                                                    views: ["year", "decade", "century"],
                                                    error: errors['performedDate'], touched: touched['performedDate'],

                                                }} handlevaluechange={handleValueChange} />
                                            </Col>}
                                    </Row>
                                    <Row>
                                        <Button className="btn-center" style={{ marginLeft: '580px' }} color="success" type="submit">Save</Button>
                                    </Row>
                                </Form>)
                            }}
                        </Formik>
                    }
                </CardBody>
            </Card>
        </Fragment>
    )
}
