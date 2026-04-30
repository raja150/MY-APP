import React, { Fragment } from 'react';
import PrivateRoute from '../../components/PrivateRoute';
import AppHeader from '../../Layout/AppHeader';
import AppSidebar from '../../Layout/AppSidebar';
import PayRunDetails from './PayRun';
import PayRunList from './PayRun/list';

const PayRun = ({ match }) => {

    return (
        <Fragment>
            {/* <ThemeOptions /> */}
            <AppHeader />
            <div className="app-main">
                <AppSidebar />
                <div className="app-main__outer">
                    <div className="app-main__inner">
                        <PrivateRoute path={`${match.url}/list`} component={PayRunList} exact />
                        <PrivateRoute path={`${match.url}/details`} component={PayRunDetails} exact />

                    </div>
                </div>
            </div>
        </Fragment>
    );
};
export default PayRun;
