import InfoDisplay from 'domain/Approvals/InfoDisplay';
import _ from 'lodash';
import moment from 'moment';
import queryString from 'query-string';
import React, { Component, Fragment } from 'react';
import ReactCSSTransitionGroup from 'react-addons-css-transition-group';
import Loader from 'react-loaders';
import { DateTimePicker } from 'react-widgets';
import momentLocalizer from 'react-widgets-moment';
import 'react-widgets/dist/css/react-widgets.css';

import avatar from 'assets/utils/images/avatars/1.jpg';
import APIService from '../../services/apiservice';
import Cards from './Cards';
import { CardTypes as CardsData } from './Cards/Types';
import PerfectScrollbar from 'react-perfect-scrollbar';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faBirthdayCake } from '@fortawesome/free-solid-svg-icons';
import * as crypto from 'utils/Crypto';
import MyCalendar from '../LeaveManagement/Leaves/Calendar'
import ApplyLeavesService from 'services/Leave/ApplyLeave';
import {
    Row, Col,
    Button,
    Nav, FormGroup,
    Container,
    NavItem,
    Card, CardBody,
    NavLink,
    Progress,
    Table,
    CardHeader,
    CardFooter,
    ButtonGroup,
    Popover,
    PopoverBody,
    ListGroupItem,
    ListGroup,
} from 'reactstrap';


import * as dateUtil from 'utils/date';
import { LEAVE_CALENDAR, MORE_DETAILS } from 'domain/Approvals/navigation';
import Loading from 'components/Loader';
moment.locale('en');
momentLocalizer();
export default class User extends Component {

    constructor(props) {
        super(props);

        this.state = {
            apiData: null,
            searchData: {},
            cardTypes: '',
            types: '',
            isLoading: true,
            empDetails: [],
            leaveDetails: [],
            approvedPending: [],
            data: [],
            isOpenCalander: false,
            date: {}
        };
    }

    componentDidMount() {
        //No Cards are displaying
        // const info = localStorage.getItem('info');
        // if (info) {
        //     const infoJson = JSON.parse(info);

        //     APIService.getAsync(`Role/${infoJson.roleId}`).then((response) => {

        //         let cardTypes = response.data && response.data.cards ?
        //             _.sortBy(response.data.cards.split(','), [function (o) { return parseInt(o); }]) : [];
        //         let idx = 0, size = 0;
        //         const cardsAry = [[]], types = [];
        //         for (let i = 0; i < cardTypes.length; i++) {
        //             const cardSize = CardsData.filter(m => m.id == cardTypes[i]);
        //             if (cardSize.length > 0) {
        //                 if ((size + cardSize[0].colsize) > 12) {
        //                     idx++;
        //                     cardsAry.push([]);
        //                     size = 0;
        //                 }
        //                 size = size + cardSize[0].colsize;
        //                 cardsAry[idx].push({ type: cardTypes[i], cardsize: cardSize[0].colsize });
        //                 types.push(cardTypes[i]);
        //             }


        //         }
        //         this.setState({ cardTypes: cardsAry, types: types });
        //     })
        // }

        // let date = new Date();
        // const dd = {
        //     from: moment(date).add(-1, 'year').format(moment.HTML5_FMT.DATE),
        //     to: moment(date).format(moment.HTML5_FMT.DATE),
        //     types: this.state.types
        // }
        // this.setState({ searchData: { ...this.state.searchData, from: dd.from, to: dd.to, 'period': 'year' } });

        // APIService.getAsync(`DashBoard?${queryString.stringify(dd)}`).then((reponse) => {
        //     this.setState({ apiData: reponse.data, isLoading: false });
        // })

        APIService.getAsync('Organization/Employee/Birthdays').then((response) => {
            this.setState({ empDetails: response.data })
        })
        APIService.getAsync('Organization/Employee/LeavesApprovedEmployees').then((response) => {
            this.setState({ leaveDetails: response.data })
        })
        APIService.getAsync('Organization/Employee/ApprovedPendingEmployees').then((response) => {
            this.setState({ approvedPending: response.data, isLoading: false })
        })
    }


    handleChange = (name, value) => {
        this.setState({ searchData: { ...this.state.searchData, [name]: value } });
    }

    handleSelectChange = (e) => {
        const date = new Date();
        if (e.target.value == 'year') {
            this.setState({ searchData: { ...this.state.searchData, from: moment(date).add(-1, 'year'), to: moment(date), 'period': e.target.value } });
        } else if (e.target.value == 'month') {
            this.setState({ searchData: { ...this.state.searchData, from: moment(date).add(-1, 'month'), to: moment(date), 'period': e.target.value } });
        } else if (e.target.value == 'week') {
            this.setState({ searchData: { ...this.state.searchData, from: moment(date).add(-7, 'day'), to: moment(date), 'period': e.target.value } });
        } else {
            this.setState({ searchData: { ...this.state.searchData, from: moment(date).add(-1, 'day'), to: moment(date), 'period': e.target.value } });
        }
    }

    handleSearch = () => {
        const dd = {
            from: moment(this.state.searchData.from).format(moment.HTML5_FMT.DATE),
            to: moment(this.state.searchData.to).format(moment.HTML5_FMT.DATE),
            types: this.state.types
        }
        APIService.getAsync(`DashBoard?${queryString.stringify(dd)}`).then((reponse) => {
            this.setState({ apiData: reponse.data });
        })
    }
    handleRedirect = async () => {
        var today = new Date(),
            date = today.getFullYear() + '-' + (today.getMonth() + 1) + '-' + today.getDate();
        const qry = {
            r: (dateUtil.getDate(date) ? crypto.encrypt(dateUtil.getDate(date)) : ''),
        };
        this.props.history.push('/Dashboard/CalendarDetails?' + queryString.stringify(qry));
    }
    render() {
        return (
            <Fragment>
                {this.state.isLoading ? <Loading /> :
                    <ReactCSSTransitionGroup
                        component="div"
                        transitionName="TabsAnimation"
                        transitionAppear={true}
                        transitionAppearTimeout={0}
                        transitionEnter={false}
                        transitionLeave={false}>
                        {/* <CardBody className="p-0 mt-2">
                        <Card className="mb-2">
                            <CardBody className="p-2"> */}
                        {/* <Row form className="w-100"> */}
                        {/* <Col md={2}>
                                        <FormGroup>
                                            <Label for="exampleEmail" className="mr-sm-2">Start Date</Label>
                                            <DateTimePicker
                                                value={this.state.searchData.from ? new Date(this.state.searchData.from) : null}
                                                onChange={(value) => this.handleChange('from', moment(value).format(moment.HTML5_FMT.DATE))}
                                                time={false}
                                            />
                                        </FormGroup>
                                    </Col>
                                <Col md={2}>
                                        <FormGroup>
                                            <Label for="examplePassword" className="mr-sm-2">End Date</Label>
                                            <DateTimePicker
                                                value={this.state.searchData.to ? new Date(this.state.searchData.to) : null}
                                                onChange={(value) => this.handleChange('to', moment(value).format(moment.HTML5_FMT.DATE))}
                                                time={false}
                                            />
                                        </FormGroup>
                                    </Col> */}

                        {/* <Col md={3}>
                                        <FormGroup>
                                            <Label className="mr-sm-2">Clinic</Label>
                                            <Input className="w-100" type="select" id="exampleCustomSelect">
                                                <option value="">Select Clinic</option>
                                                <option>Value 1</option>
                                                <option>Value 2</option>
                                                <option>Value 3</option>
                                                <option>Value 4</option>
                                                <option>Value 5</option>
                                            </Input>
                                        </FormGroup>
                                    </Col>
                                <Col md={3}>
                                        <FormGroup>
                                            <Label className="mr-sm-2">Select Period</Label>
                                            <Input name="period" className="w-100 form-control form-control-sm" type="select" id="exampleCustomSelect"
                                                onChange={this.handleSelectChange}
                                                value={this.state.searchData.period}>
                                                <option value="">Select Period</option>
                                                <option value="week">Last Week</option>
                                                <option value="month">Last Month</option>
                                                <option value="year">Last Year</option>
                                            </Input>
                                        </FormGroup>
                                    </Col>
                                <Col md={2} className="mt-1">
                                        <Button className="mt-3 mr-2 btn-icon btn-icon-only btn-secondary btn-sm" onClick={this.handleSearch}>
                                            <i className="pe-7s-search btn-icon-wrapper"> </i></Button>
                                    </Col>  */}
                        {/* </Row> */}

                        {/* </CardBody>
                        </Card>
                        {this.state.isLoading ?
                            <Loader type="ball-grid-pulse" /> :
                            <Cards apiData={this.state.apiData} cardTypes={this.state.cardTypes} />} */}
                        {/* <Row>
                            <Col md="12">
                                <Card className="main-card mb-2">
                                    <CardBody>
                                        <CardTitle>Bar Charts</CardTitle>
                                        <ResponsiveContainer width='100%' aspect={4.0 / 3.0}>
                                            <BarChart data={data}>
                                                <CartesianGrid strokeDasharray="3 3" />
                                                <XAxis dataKey="name" />
                                                <YAxis />
                                                <Tooltip />
                                                <Legend />
                                                <Bar dataKey="pv" fill="#fece78" />
                                                <Bar dataKey="uv" fill="#82ca9d" />
                                            </BarChart>
                                        </ResponsiveContainer>
                                    </CardBody>
                                </Card>
                            </Col>
                        </Row> */}

                        {/* </CardBody> */}
                        <Row>
                            {/* <Col sm="12" lg="4">
                            <Card className="card-hover-shadow-2x mb-3">
                                <CardHeader className="card-header-tab">
                                    <div
                                        className="card-header-title font-size-lg text-capitalize font-weight-normal">
                                       <FontAwesomeIcon icon={faBirthdayCake} fontSizeAdjust='15px'/>
                                        <h3 style={{ marginLeft: '3px' }}>Birthdays List</h3>
                                    </div>
                                </CardHeader>
                                <div className="scroll-area-lg">
                                    <PerfectScrollbar>
                                        <div className="p-2">
                                            <ListGroup className="todo-list-wrapper" flush>
                                                {this.state.empDetails.map((dob, index) => {
                                                    return (
                                                        <h6 key={index}>{dob.name}</h6>
                                                    )
                                                })}
                                            </ListGroup>
                                        </div>
                                    </PerfectScrollbar>
                                </div>

                            </Card>
                        </Col> */}

                            <Col sm="12" lg="4">
                                <Card className="main-card mb-3 card-hover-shadow-2x">
                                    <CardHeader className="card-header-tab">
                                        <i className="header-icon lnr-gift icon-gradient bg-ripe-malin" />
                                        Birthdays
                                    </CardHeader>
                                    <CardBody>
                                        <Card className="border-light card-border scroll-area-sm">
                                            <PerfectScrollbar>
                                                <ListGroup flush >

                                                    {this.state.empDetails.length > 0 ? this.state.empDetails.map((e, index) => {
                                                        return (
                                                            <ListGroupItem key={index} className="border-bottom-0">
                                                                <div className="widget-content p-0">
                                                                    <div className="widget-content-wrapper">
                                                                        <div className="widget-content-left mr-3">
                                                                            <img width={28}
                                                                                className="rounded-circle"
                                                                                src={`${process.env.REACT_APP_IMAGE_ENDPOINT}/avatars/${e.id}.jpg`}
                                                                                onError={(e) => (e.target.onerror = null, e.target.src = avatar)}
                                                                                alt="" />
                                                                        </div>
                                                                        <div className="widget-content-left">
                                                                            <div className="widget-heading">
                                                                                {e.name}
                                                                            </div>
                                                                        </div>
                                                                        <div className="widget-content-right">
                                                                            <div
                                                                                className="text-success">
                                                                                {dateUtil.getDayMonth(e.dateOfBirth)}
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </ListGroupItem>

                                                        )
                                                    })
                                                        : <p style={{ fontSize: "15px" }}>No BirthDays in this week</p>
                                                    }
                                                </ListGroup>
                                            </PerfectScrollbar>
                                        </Card>
                                    </CardBody>
                                </Card>
                            </Col>
                            <Col sm="12" lg="4">
                                <Card className="card-hover-shadow-2x mb-3">
                                    <CardHeader className="card-header-tab">
                                        <Row>
                                            <i className="header-icon lnr-home icon-gradient bg-ripe-malin" />
                                            On Leave Today
                                            <Col style={{ marginLeft: '150px' }}>
                                                <a href='javascript:void()' onClick={(e) => this.handleRedirect(e)}>
                                                    <i className="lnr lnr-calendar-full" /> More</a>
                                            </Col>
                                        </Row>
                                    </CardHeader>
                                    <div className="scroll-area-lg">
                                        {this.state.leaveDetails.map((leave, index) => {
                                            return (
                                                <h6 key={index}>{leave.name}</h6>
                                            )
                                        })}
                                    </div>
                                </Card>
                            </Col>
                            <Col sm="12" lg="4">
                                <Card className="card-hover-shadow-2x mb-3">
                                    <CardHeader className="card-header-tab">
                                        <i className="header-icon lnr-apartment icon-gradient bg-ripe-malin" />
                                        Request For Approval
                                    </CardHeader>
                                    <div className="scroll-area-lg">
                                        {this.state.approvedPending.map((pending, index) => {
                                            return (
                                                <h6 key={index}>{pending.name}</h6>
                                            )
                                        })}
                                    </div>
                                </Card>
                            </Col>
                        </Row>
                        {this.state.isOpenCalander && <MyCalendar data={this.state.data} month={moment(this.state.date).format('MMMM')} />}
                    </ReactCSSTransitionGroup>
                }
            </Fragment>
        )

    }

}
