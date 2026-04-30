import React, { Fragment } from 'react';
import PrivateRoute from '../../components/PrivateRoute';
import AppHeader from '../../Layout/AppHeader';
import AppSidebar from '../../Layout/AppSidebar';
import CodingDataImport from './DataImport';

const Coding = ({ match }) => {

    return (
        <Fragment>
            {/* <ThemeOptions /> */}
            <AppHeader />
            <div className="app-main">
                <AppSidebar />
                <div className="app-main__outer">
                    <div className="app-main__inner">
                        <PrivateRoute path={`${match.url}/Import`} component={CodingDataImport} exact/>

                    </div>
                </div>
            </div>
        </Fragment>
    );
};
export default Coding;
