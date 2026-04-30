import { faBusinessTime } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import cx from 'classnames';
import Loading from 'components/Loader';
import React, { Fragment, useEffect, useState } from 'react';
import ReactCSSTransitionGroup from 'react-addons-css-transition-group';
import {
    Card, CardBody, CardHeader, Col, DropdownMenu, DropdownToggle,
    Nav, NavItem, NavLink, Row, TabContent, TabPane, UncontrolledDropdown
} from 'reactstrap';
import ReportService from 'services/SelfService/ReportService';
import { APPLY_LEAVE, APPLY_WFH } from '../navigation';
import HolidaysList from './HolidaysList';
import PastHolidaysList from './PastHolidaysList';
import LeaveRequestDetails from './LeaveRequestDetails';
import LeaveBalanceDetails from './LeaveBalanceDetails';

function LeaveBalance(props) {
    const [state, setState] = useState({ isLoading: true, leaveBal: [], })
    const [activeTab, setActiveTab] = useState('1')
    useEffect(() => {
        const fetchData = async () => {
            let leaveBal = []
            await ReportService.getLeaveBalance().then((result) => {
                leaveBal = result.data
            })
            setState({ ...state, isLoading: false, leaveBal: leaveBal, });
        }
        fetchData();
    }, [])

    const handleClick = async () => {
        props.history.push(APPLY_LEAVE)
    }
    const handleClick1 = async () => {
        props.history.push(APPLY_WFH)
    }
    const toggle = (tab) => {
        if (activeTab !== tab) {
            setActiveTab(tab);
        }
    }

    return (
        state.isLoading ? <Loading /> :
            <Fragment>
                <div className="app-page-title">
                    <div className="page-title-wrapper">
                        <div className="page-title-heading">
                            <div
                                className={cx("page-title-icon rounded-circle")}>
                                <i className="pe-7s-drawer icon-gradient bg-happy-itmeo" />
                            </div>
                            <div>
                                <div>Leave Balance</div>
                                <div className={cx("page-title-subheading")}>
                                    Here are your leave balances as of today.
                                </div>
                            </div>
                        </div>
                        <div className="page-title-actions">
                            <UncontrolledDropdown className="d-inline-block">
                                <DropdownToggle color="info" className="btn-shadow" caret>
                                    <span className="btn-icon-wrapper pr-2 opacity-7">
                                        <FontAwesomeIcon icon={faBusinessTime} />
                                    </span>
                                    Apply
                                </DropdownToggle>
                                <DropdownMenu right>
                                    <Nav vertical>
                                        <NavItem>
                                            <NavLink onClick={handleClick}>
                                                <span>Leave</span>
                                            </NavLink>
                                        </NavItem>
                                        <NavItem>
                                            <NavLink onClick={handleClick1}>
                                                <span>Work from home</span>
                                            </NavLink>
                                        </NavItem>
                                    </Nav>
                                </DropdownMenu>
                            </UncontrolledDropdown>
                        </div>
                    </div>
                </div>
                <ReactCSSTransitionGroup
                    component="div"
                    transitionName="TabsAnimation"
                    transitionAppear={true}
                    transitionAppearTimeout={0}
                    transitionEnter={false}
                    transitionLeave={false}>
                    <Row>
                        {state.leaveBal.length > 0 && state.leaveBal.map((type, index) => {
                            return (
                                <Col sm="8" md="2" key={index}>
                                    <Card className="card-shadow-s mb-3 widget-chart widget-chart2 text-center">
                                        <h6 className="card-header-title font-size-lg text-capitalize">
                                            {type.leaveType}
                                        </h6>
                                        <div className="widget-numbers mb-0 w-100">
                                            <div className="card-shadow-s mb-0 widget-chart widget-chart2 text-center">{type.code}</div>
                                            <div className='widget-content-left'>
                                                {type.available} <br />
                                            </div>
                                        </div>
                                    </Card>
                                </Col>)
                        })}
                    </Row>
                </ReactCSSTransitionGroup>
                <Card tabs="true" className="mb-3">
                    <CardHeader className="card-header-tab">
                        <Nav>
                            <NavItem>
                                <NavLink href="javascript:void(0);"
                                    className={cx({ active: activeTab === '1' })}
                                    onClick={() => {
                                        toggle('1');
                                    }}
                                >
                                    Upcoming Holidays
                                </NavLink>
                            </NavItem>
                            <NavItem>
                                <NavLink href="javascript:void(0);"
                                    className={cx({ active: activeTab === '2' })}
                                    onClick={() => {
                                        toggle('2');
                                    }}
                                >
                                    Holidays History
                                </NavLink>
                            </NavItem>
                            <NavItem>
                                <NavLink href="javascript:void(0);"
                                    className={cx({ active: activeTab === '3' })}
                                    onClick={() => {
                                        toggle('3');
                                    }}
                                >
                                    Your Leaves
                                </NavLink>
                            </NavItem>
                            <NavItem>
                                <NavLink href="javascript:void(0);"
                                    className={cx({ active: activeTab === '4' })}
                                    onClick={() => {
                                        toggle('4');
                                    }}
                                >
                                    Your Leave Balance Breakup
                                </NavLink>
                            </NavItem>
                        </Nav>
                    </CardHeader>
                    <CardBody>

                        <TabContent activeTab={activeTab}>
                            {activeTab === '1' && <TabPane tabId="1">
                                <HolidaysList {...props} />
                            </TabPane>}
                            {activeTab === '2' && <TabPane tabId="2">
                                <PastHolidaysList {...props} />
                            </TabPane>}
                            {activeTab === '3' && <TabPane tabId="3">
                                <LeaveRequestDetails {...props} />
                            </TabPane>}
                            {activeTab === '4' && <TabPane tabId="4">
                                <LeaveBalanceDetails {...props} />
                            </TabPane>}
                        </TabContent>

                    </CardBody>
                </Card>
            </Fragment>
    )
}
export default LeaveBalance
