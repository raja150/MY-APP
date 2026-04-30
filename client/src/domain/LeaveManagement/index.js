import React, { Fragment } from 'react';
import PrivateRoute from '../../components/PrivateRoute';
import AppHeader from '../../Layout/AppHeader';
import AppSidebar from '../../Layout/AppSidebar';
import LeaveDataImport from './DataImport';
import ApplyWFH from './ApplyWFH';
import WFHList from './ApplyWFH/List'

import WeekOffSetup from './WeekOffSetup/List'
import WeekOffSetupForm from './WeekOffSetup';
import LMApplyLeaves from './Leaves/ApplyLeave';
import LeavesList from '../LeaveManagement/Leaves/List'
import LMDetails from './Leaves/Details';
import MyCalendar from './Leaves/Calendar';
import Attendance from './Attendance/Tabs';
import UnAuthorizedLeavesForm from './UnAuthorizedLeaves';
import UnAuthorizedLeaves from './UnAuthorizedLeaves/List';
import ApplyClientVisits from './ApplyClientVisits';
import ApplyClientVisitList from './ApplyClientVisits/List'

const Leave = ({ match }) => {

    return (
        <Fragment>
            {/* <ThemeOptions /> */}
            <AppHeader />
            <div className="app-main">
                <AppSidebar />
                <div className="app-main__outer">
                    <div className="app-main__inner">
                        <PrivateRoute path={`${match.url}/Import`} component={LeaveDataImport} exact />
                        <PrivateRoute path={`${match.url}/WFH/New`} component={ApplyWFH} exact />
                        {/* <PrivateRoute path={`${match.url}/WFH/Update`} component={ApplyWFH} exact /> */}
                        <PrivateRoute path={`${match.url}/WFH`} component={WFHList} exact />
                        <PrivateRoute path={`${match.url}/WeekOffSetup`} component={WeekOffSetup} exact />
                        <PrivateRoute path={`${match.url}/WeekOffSetup/New`} component={WeekOffSetupForm} exact />
                        <PrivateRoute path={`${match.url}/WeekOffSetup/Update`} component={WeekOffSetupForm} exact />

                        <PrivateRoute path={`${match.url}/UnAuthorized`} component={UnAuthorizedLeaves} exact />
                        <PrivateRoute path={`${match.url}/UnAuthorized/New`} component={UnAuthorizedLeavesForm} exact />
                        <PrivateRoute path={`${match.url}/UnAuthorized/Update`} component={UnAuthorizedLeavesForm} exact />
                        
                        <PrivateRoute path={`${match.url}/Calendar`} component={MyCalendar} />
                        <PrivateRoute path={`${match.url}/Leaves`} component={LeavesList} exact />
                        <PrivateRoute path={`${match.url}/Leaves/New`} component={LMApplyLeaves} exact />
                        <PrivateRoute path={`${match.url}/ApproveLeave/Details`} component={LMDetails} exact />

                        <PrivateRoute path={`${match.url}/Attendance`} component={Attendance} exact />
                        
                        <PrivateRoute path={`${match.url}/ApplyClientVisits`} component={ApplyClientVisitList} exact />
                        <PrivateRoute path={`${match.url}/ApplyClientVisits/New`} component={ApplyClientVisits} exact />
                    </div>
                </div>
            </div>
        </Fragment>
    );
};
export default Leave;
