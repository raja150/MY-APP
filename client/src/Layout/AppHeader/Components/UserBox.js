import { faAngleDown } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import React, { Fragment } from 'react';
import { useHistory } from 'react-router-dom';
import { Button, DropdownMenu, DropdownToggle, UncontrolledButtonDropdown } from 'reactstrap';
import city3 from '../../../assets/utils/images/dropdown-header/city3.jpg';
import SessionStorageService from 'services/SessionStorage';

import avatar from 'assets/utils/images/avatars/1.jpg';

export default function UserBox(props) {
    let history = useHistory();

    const handleLogout = (e) => {
        e.preventDefault();
        SessionStorageService.removeUser();
        SessionStorageService.removeRefreshToken();
        SessionStorageService.removeSearchData();
        SessionStorageService.removePwdExpire();
        SessionStorageService.removeUserInfo();

        history.push('/login')
    }

    const handleClick = () => {
        history.push('/Dashboard/Details')
    }

    const handleChangePassword = (e) => {
        history.push('/changePassword')
    }

    const info = SessionStorageService.getUserInfo();

    let infoJson = {};
    if (info) {
        infoJson = JSON.parse(info);
    }

    return (
        <Fragment>
            <div className="header-btn-lg pr-0">
                <div className="widget-content p-0">
                    <div className="widget-content-wrapper">
                        <div className="widget-content-left">
                            <UncontrolledButtonDropdown>
                                <DropdownToggle color="link" className="p-0">
                                    <img width={42} className="rounded-circle"
                                        src={`${process.env.REACT_APP_IMAGE_ENDPOINT}/avatars/${infoJson.image}.jpg`} alt=""
                                        onError={(e) => (e.target.onerror = null, e.target.src = avatar)}
                                    />
                                    <FontAwesomeIcon className="ml-2 opacity-8" icon={faAngleDown} />
                                </DropdownToggle>
                                <DropdownMenu right className="rm-pointers dropdown-menu-lg pb-0">
                                    <div className="dropdown-menu-header mb-0">
                                        <div className="dropdown-menu-header-inner btn-primary1">
                                            <div className="menu-header-image opacity-2"
                                                style={{
                                                    backgroundImage: 'url(' + city3 + ')'
                                                }}
                                            />
                                            <div className="menu-header-content text-left">
                                                <div className="widget-content p-0">
                                                    <div className="widget-content-wrapper d-block">
                                                        <div className="text-center">
                                                            <button type="button" onClick={handleClick}>
                                                                <img width={42} className="rounded-circle"
                                                                    src={`${process.env.REACT_APP_IMAGE_ENDPOINT}/avatars/${infoJson.image || 'noImage'}.jpg`}
                                                                    onError={(e) => (e.target.onerror = null, e.target.src = avatar)} alt="" />
                                                            </button>
                                                            <div>
                                                                <div>
                                                                    {infoJson && infoJson.name}
                                                                </div>
                                                                <div className='my-2'>
                                                                    {/* {"-"} */}
                                                                    <a href='#/changePassword' style={{ color: '#fff' }} >
                                                                        Change Password
                                                                    </a>
                                                                </div>
                                                            </div>

                                                            <div>
                                                                <Button className="btn-pill btn-focus1" onClick={handleLogout}>
                                                                    Logout
                                                                </Button>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    {/* <div className="scroll-area-xs" style={{
                                        height: '150px'
                                    }}>
                                        <PerfectScrollbar>
                                            <Nav vertical>
                                                <NavItem className="nav-item-header">
                                                    Activity
                                                    </NavItem>
                                                <NavItem>
                                                    <NavLink href="#/">
                                                        Chat
                                                            <div className="ml-auto badge badge-pill badge-info">8</div>
                                                    </NavLink>
                                                </NavItem>
                                                <NavItem>
                                                    <NavLink href="#/">Recover Password</NavLink>
                                                </NavItem>
                                                <NavItem className="nav-item-header">
                                                    My Account
                                                    </NavItem>
                                                <NavItem>
                                                    <NavLink href="#/">
                                                        Settings
                                                            <div className="ml-auto badge badge-success">New</div>
                                                    </NavLink>
                                                </NavItem>
                                                <NavItem>
                                                    <NavLink href="#/">
                                                        Messages
                                                            <div className="ml-auto badge badge-warning">512</div>
                                                    </NavLink>
                                                </NavItem>
                                                <NavItem>
                                                    <NavLink href="#/">
                                                        Logs
                                                        </NavLink>
                                                </NavItem>
                                            </Nav>
                                        </PerfectScrollbar>
                                    </div> */}
                                    {/* <Nav vertical>
                                        <NavItem className="nav-item-divider mb-0" />
                                    </Nav> */}
                                    {/* <div className="grid-menu grid-menu-2col">
                                        <Row className="no-gutters">
                                            <Col sm="6">
                                                <Button
                                                    className="btn-icon-vertical btn-transition btn-transition-alt pt-2 pb-2"
                                                    outline color="warning">
                                                    <i className="pe-7s-chat icon-gradient bg-amy-crisp btn-icon-wrapper mb-2"> </i>
                                                    Message Inbox
                                                    </Button>
                                            </Col>
                                            <Col sm="6">
                                                <Button
                                                    className="btn-icon-vertical btn-transition btn-transition-alt pt-2 pb-2"
                                                    outline color="danger">
                                                    <i className="pe-7s-ticket icon-gradient bg-love-kiss btn-icon-wrapper mb-2"> </i>
                                                    <b>Support Tickets</b>
                                                </Button>
                                            </Col>
                                        </Row>
                                    </div>
                                    <Nav vertical>
                                        <NavItem className="nav-item-divider" />
                                        <NavItem className="nav-item-btn text-center">
                                            <Button size="sm" className="btn-wide" color="primary">
                                                Open Messages
                                                </Button>
                                        </NavItem>
                                    </Nav> */}
                                </DropdownMenu>
                            </UncontrolledButtonDropdown>
                        </div>
                        <div className="widget-content-left  ml-3 header-user-info">
                            <div className="widget-heading">
                                {infoJson && infoJson.name}
                            </div>
                            <div className="widget-subheading">
                                {"-"}
                            </div>
                        </div>

                        {/* <div className="widget-content-right header-user-info ml-3">
                                <Button className="btn-shadow p-1" size="sm" onClick={this.notify2} color="info"
                                        id="Tooltip-1">
                                    <Ionicon color="#ffffff" fontSize="20px" icon="ios-calendar-outline"/>
                                </Button>
                                <UncontrolledTooltip placement="bottom" target={'Tooltip-1'}>
                                    Click for Toastify Notifications!
                                </UncontrolledTooltip>
                            </div> */}
                    </div>
                </div>
            </div>
        </Fragment>
    )

}
