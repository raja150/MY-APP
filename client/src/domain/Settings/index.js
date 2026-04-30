import React, { Fragment } from 'react';
import PrivateRoute from '../../components/PrivateRoute';
import AppHeader from '../../Layout/AppHeader';
import AppSidebar from '../../Layout/AppSidebar';
import AutoNo from './AutoNo';
import AddOrEdit from './Roles/AddOrEdit';
import AddOrEditNew from './Roles/AddOrEditNew';
import RoleList from './Roles/List';

const Supplies = ({ match }) => (
    <Fragment>
        {/* <ThemeOptions /> */}
        <AppHeader />
        <div className="app-main">
            <AppSidebar />
            <div className="app-main__outer">
                <div className="app-main__inner">
                    <PrivateRoute path={`${match.url}/AutoNo`} component={AutoNo} exact /> 
                    <PrivateRoute path={`${match.url}/Role/New`} component={AddOrEditNew} exact />
                    <PrivateRoute path={`${match.url}/Role/Update`} component={AddOrEditNew} exact />
                    <PrivateRoute path={`${match.url}/Role`} component={RoleList} exact />
                </div>

            </div>
        </div>
    </Fragment>
);

export default Supplies;