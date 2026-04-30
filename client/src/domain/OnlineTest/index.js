import React, { Fragment } from 'react';
import PrivateRoute from '../../components/PrivateRoute';
import AppHeader from '../../Layout/AppHeader';
import AppSidebar from '../../Layout/AppSidebar';
import Questions from './Questions/index';
import QuestionsList from './Questions/List';
import List from './Paper/List'
import PaperDetails from './Paper/index';
import Correction from './Correction/index'
import CorrectionList from './Correction/List';
import ResultList from './Result/List'
import Summary from './Result/Summary';

const OnlineTest = ({ match }) => (
    <Fragment>
        {/* <ThemeOptions /> */}
        <AppHeader />
        <div className="app-main">
            <AppSidebar />
            <div className="app-main__outer">
                <div className="app-main__inner">
                    <PrivateRoute path={`${match.url}/Question/New`} component={Questions} exact />
                    <PrivateRoute path={`${match.url}/Question/Update`} component={Questions} exact />
                    <PrivateRoute path={`${match.url}/Question`} component={QuestionsList} exact />
                    <PrivateRoute path={`${match.url}/Paper`} component={List} exact />
                    <PrivateRoute path={`${match.url}/Paper/Update`} component={PaperDetails} exact />
                    <PrivateRoute path={`${match.url}/Paper/New`} component={PaperDetails} exact />
                    <PrivateRoute path={`${match.url}/Correction`} component={CorrectionList} exact />
                    <PrivateRoute path={`${match.url}/Correction/Verify`} component={Correction} exact />
                    <PrivateRoute path={`${match.url}/Result`} component={ResultList} exact />
                    <PrivateRoute path={`${match.url}/Summary`} component={Summary} exact />

                </div>
            </div>
        </div>
    </Fragment>
);

export default OnlineTest;