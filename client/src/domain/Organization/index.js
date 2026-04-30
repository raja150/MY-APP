import React, { Fragment } from 'react';
import PrivateRoute from '../../components/PrivateRoute';
import AppHeader from '../../Layout/AppHeader';
import AppSidebar from '../../Layout/AppSidebar';
import UserProfile from './Image';
import Users from './Users';
import UsersList from './Users/List';
import ImageDetailsList from './ImageDetails/List';
import ImageDetails from './ImageDetails';
import EmployeeDevice from './EmployeeDevice';
import EmployeeDeviceList from './EmployeeDevice/list';
import Performance from './Performance'
import EmployeePerformanceList from './Performance/List'

const OrgUsers = ({ match }) => (
    <Fragment>
        <AppHeader />
        <div className="app-main">
            <AppSidebar />
            <div className="app-main__outer">
                <div className="app-main__inner">
                        <PrivateRoute path={`${match.url}/Users`} component={UsersList} exact />
                        <PrivateRoute path={`${match.url}/Users/New`} component={Users} exact />
                        <PrivateRoute path={`${match.url}/Users/Update`} component={Users} exact />
                        <PrivateRoute path={`${match.url}/Users/Profile`} component={UserProfile} exact />

                        <PrivateRoute path={`${match.url}/EmployeeImage/Update`} component={ImageDetails} exact />
                        <PrivateRoute path={`${match.url}/EmployeeImage`} component={ImageDetailsList} exact />

                        <PrivateRoute path={`${match.url}/EmployeeDevice`} component={EmployeeDeviceList} exact />
                        <PrivateRoute path={`${match.url}/EmployeeDevice/New`} component={EmployeeDevice} exact />
                        <PrivateRoute path={`${match.url}/EmployeeDevice/Update`} component={EmployeeDevice} exact />

                        <PrivateRoute path={`${match.url}/Performance`} component={EmployeePerformanceList} exact />
                        <PrivateRoute path={`${match.url}/Performance/New`} component={Performance} exact />
                        <PrivateRoute path={`${match.url}/Performance/Update`} component={Performance} exact />
                </div>
            </div>
        </div>
    </Fragment>
);

export default OrgUsers;
