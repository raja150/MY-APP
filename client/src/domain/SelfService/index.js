import React, { Fragment } from 'react';
import PrivateRoute from '../../components/PrivateRoute';
import AppHeader from '../../Layout/AppHeader';
import AppSidebar from '../../Layout/AppSidebar';
import ApplyCompensatoryWorkingDay from './ApplyCompensatoryWorkingDay';
import ApplyLeaves from './ApplyLeaves';
import ApplyLeavesList from './ApplyLeaves/List';
import SelfDeclaration from './Declarations';
import LeaveBalance from './Leaves/Balance';
import Payslips from './Payslips';
import RaiseTicketList from './Ticket/List';
import ApplyWFH from './WFH';
import WorkFromHomeList from './WFH/List';
import WorkingDayList from './ApplyCompensatoryWorkingDay/List';
import ApplyClientVisits from './ApplyClientVisits';
import ApplyClientVisitList from './ApplyClientVisits/List'
import Attendance from './Attendance';
import Ticket from './Ticket'; 
import EditResponse from './Ticket/Edit';
import Test from './Test/List';
import Result from './Result/List'
import Summary from 'domain/OnlineTest/Result/Summary';
import Compliances from './Compliance';

const SelfService = ({ match }) => {

    return (
        <Fragment>
            {/* <ThemeOptions /> */}
            <AppHeader />
            <div className="app-main">
                <AppSidebar />
                <div className="app-main__outer">
                    <div className="app-main__inner">
                        <PrivateRoute path={`${match.url}/LeaveBalance`} component={LeaveBalance} exact />
                        <PrivateRoute path={`${match.url}/SalaryDetails`} component={Payslips} exact />
                        {/* <PrivateRoute path={`${match.url}/Payslips/screen`} component={Payslips} exact /> */}
                        <PrivateRoute path={`${match.url}/Declaration`} component={SelfDeclaration} exact />
                        {/* <PrivateRoute path={`${match.url}/Payslips/screen`} component={Payslips} exact /> */}
                        <PrivateRoute path={`${match.url}/ApplyWFH/New`} component={ApplyWFH} exact />
                        <PrivateRoute path={`${match.url}/ApplyWFH/Update`} component={ApplyWFH} exact />
                        <PrivateRoute path={`${match.url}/ApplyWFH`} component={WorkFromHomeList} exact />
                        <PrivateRoute path={`${match.url}/ApplyLeaves/New`} component={ApplyLeaves} exact />
                        <PrivateRoute path={`${match.url}/ApplyLeaves/Update`} component={ApplyLeaves} exact />
                        <PrivateRoute path={`${match.url}/ApplyLeaves`} component={ApplyLeavesList} exact />
                        <PrivateRoute path={`${match.url}/RaiseTicket/New`} component={Ticket} exact />
                        <PrivateRoute path={`${match.url}/RaiseTicket/Update`} component={Ticket} exact />
                        <PrivateRoute path={`${match.url}/RaiseTicket`} component={RaiseTicketList} exact />
                        <PrivateRoute path={`${match.url}/RaiseTicket/Edit`} component={EditResponse} exact />
                        <PrivateRoute path={`${match.url}/ApplyCompensatoryWorkingDay/new`} component={ApplyCompensatoryWorkingDay} exact />
                        <PrivateRoute path={`${match.url}/ApplyCompensatoryWorkingDay`} component={WorkingDayList} exact />
                        <PrivateRoute path={`${match.url}/ApplyClientVisits`} component={ApplyClientVisitList} exact />
                        <PrivateRoute path={`${match.url}/ApplyClientVisits/New`} component={ApplyClientVisits} exact />
                        <PrivateRoute path={`${match.url}/ApplyClientVisits/Update`} component={ApplyClientVisits} exact />
                        <PrivateRoute path={`${match.url}/Attendance`} component={Attendance} exact />
                        <PrivateRoute path={`${match.url}/Test`} component={Test} exact />
                        <PrivateRoute path={`${match.url}/Result`} component={Result} exact />
                        <PrivateRoute path={`${match.url}/Summary`} component={Summary} exact />
                        <PrivateRoute path={`${match.url}/Compliances`} component={Compliances} exact />  

                    </div>
                </div>
            </div>
        </Fragment>
    );
};
export default SelfService;
