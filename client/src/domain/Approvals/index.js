import React, { Fragment } from 'react';
import PrivateRoute from '../../components/PrivateRoute';
import AppHeader from '../../Layout/AppHeader';
import AppSidebar from '../../Layout/AppSidebar';
import CompensatoryWork from './ApplyCompensatory';
import CompensatoryWorkList from './ApplyCompensatory/list';
import ClientVisitList from './ClientVisit/List';
// import Ticket from './Ticket';
// import TicketList from './Ticket/List';
import WFH from './WFH';
import WFHList from './WFH/List';
import ApprovalLeavesList from '../LeaveManagement/LeaveApprovals/List'
import LeaveApplication from '../LeaveManagement/LeaveApprovals/Approve';
import ApprovalDetails from '../LeaveManagement/LeaveApprovals/Details';
import WebAttendanceList from '../Approvals/WebAttendance/List'
import MyTeamAttendance from '../Approvals/MyTeamAttendance'
const Approvals = ({ match }) => {

    return (
        <Fragment>
            {/* <ThemeOptions /> */}
            <AppHeader />
            <div className="app-main">
                <AppSidebar />
                <div className="app-main__outer">
                    <div className="app-main__inner">

                        <PrivateRoute path={`${match.url}/clientVisit`} component={ClientVisitList} exact />

                        <PrivateRoute path={`${match.url}/wfh`} component={WFHList} exact />
                        <PrivateRoute path={`${match.url}/wfh/update`} component={WFH} exact />

                        <PrivateRoute path={`${match.url}/compensatory`} component={CompensatoryWorkList} exact />
                        <PrivateRoute path={`${match.url}/compensatory/New`} component={CompensatoryWork} exact />
                        <PrivateRoute path={`${match.url}/compensatory/Update`} component={CompensatoryWork} exact />

                        {/* <PrivateRoute path={`${match.url}/Ticket/New`} component={Ticket} exact /> */}
                        {/* <PrivateRoute path={`${match.url}/Ticket`} component={TicketList} exact /> */}

                        <PrivateRoute path={`${match.url}/leave`} component={ApprovalLeavesList} exact />
                        <PrivateRoute path={`${match.url}/leave/update`} component={LeaveApplication} exact />
                        <PrivateRoute path={`${match.url}/Details`} component={ApprovalDetails} exact />

                        <PrivateRoute path={`${match.url}/WebAttendance`} component={WebAttendanceList} exact />
                        <PrivateRoute path={`${match.url}/MyTeamAttendance`} component ={MyTeamAttendance} exact />

                    </div>
                </div>
            </div>
        </Fragment>
    );
};
export default Approvals;
