import React, { Fragment } from 'react';
import PrivateRoute from '../../components/PrivateRoute';
import AppHeader from '../../Layout/AppHeader';
import AppSidebar from '../../Layout/AppSidebar';
import PayRunDetails from '../PayRuns/PayRun';
import PayRunList from '../PayRuns/PayRun/list';
import PayRollDataImport from './DataImport';
import IncentivesPayCut from './IncentivesPayCut';
import PayRollDeclaration from './ITDeclaration/Add';
import EmployeeTaxDetailsList from './ITDeclaration/DetailsList';
import DeclarationTable from './ITDeclaration/Update';
// import Declaration from './ITDeclaration'
import Salary from './Salary';
import SalaryList from './Salary/List';
import IncentivesPayCutList from './IncentivesPayCut/List'
import Arrears from './Arrears';
import ArrearsList from './Arrears/List';
import PaymentInfoList from './PaymentInfo/List';
import IncomeTaxLimit from './IncomeTaxLimit/Index';
import IncomeTaxLimitList from './IncomeTaxLimit/List'
import LateComing from './Latecomers';
import LatecomersList from './Latecomers/List'
import PaymentInfo from './PaymentInfo';


const PayRoll = ({ match }) => {

    return (
        <Fragment>
            {/* <ThemeOptions /> */}
            <AppHeader />
            <div className="app-main">
                <AppSidebar />
                <div className="app-main__outer">
                    <div className="app-main__inner">
                        <PrivateRoute path={`${match.url}/Salary`} component={SalaryList} exact />
                        <PrivateRoute path={`${match.url}/Salary/new`} component={Salary} exact />
                        <PrivateRoute path={`${match.url}/Salary/update`} component={Salary} exact />
                        <PrivateRoute path={`${match.url}/Declaration/new`} component={PayRollDeclaration} exact />
                        {/* <PrivateRoute path={`${match.url}/Declaration`} component={PayRollList} exact/> */}
                        <PrivateRoute path={`${match.url}/Declaration/Update`} component={DeclarationTable} exact />
                        {/* <PrivateRoute path={`${match.url}/Declaration/updateTable`} component={PayRollDeclaration} exact/> */}

                        <PrivateRoute path={`${match.url}/Table`} component={DeclarationTable} exact />
                        <PrivateRoute path={`${match.url}/Declaration`} component={EmployeeTaxDetailsList} exact />

                        <PrivateRoute path={`${match.url}/PayRun`} component={PayRunList} exact />
                        <PrivateRoute path={`${match.url}/PayRun/details`} component={PayRunDetails} exact />
                        <PrivateRoute path={`${match.url}/Import`} component={PayRollDataImport} exact />
                        <PrivateRoute path={`${match.url}/IncentivePayCut/New`} component={IncentivesPayCut} exact />
                        <PrivateRoute path={`${match.url}/IncentivePayCut`} component={IncentivesPayCutList} exact />
                        <PrivateRoute path={`${match.url}/IncentivePayCut/Update`} component={IncentivesPayCut} exact />
                        <PrivateRoute path={`${match.url}/Arrears`} component={ArrearsList} exact />
                        <PrivateRoute path={`${match.url}/Arrears/New`} component={Arrears} exact />
                        <PrivateRoute path={`${match.url}/Arrears/Update`} component={Arrears} exact />

                        <PrivateRoute path={`${match.url}/EmployeePayInfo`} component={PaymentInfoList} exact />
                        <PrivateRoute path={`${match.url}/EmployeePayInfo/New`} component={PaymentInfo} exact />
                        <PrivateRoute path={`${match.url}/EmployeePayInfo/Update`} component={PaymentInfo} exact />

                        <PrivateRoute path={`${match.url}/IncomeTaxLimit`} component={IncomeTaxLimitList} exact />
                        <PrivateRoute path={`${match.url}/IncomeTaxLimit/New`} component={IncomeTaxLimit} exact />
                        <PrivateRoute path={`${match.url}/IncomeTaxLimit/Update`} component={IncomeTaxLimit} exact />
                        <PrivateRoute path={`${match.url}/Latecomer`} component={LatecomersList} exact />
                        <PrivateRoute path={`${match.url}/Latecomer/New`} component={LateComing} exact />
                        <PrivateRoute path={`${match.url}/Latecomer/Update`} component={LateComing} exact />
                    </div>
                </div>
            </div>
        </Fragment>
    );
};
export default PayRoll;
