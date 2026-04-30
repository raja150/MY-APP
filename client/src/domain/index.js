import React, { Fragment, lazy, Suspense } from 'react';
import Loader from 'react-loaders';
import { Route } from 'react-router-dom';

//import Reports from 'Pages/Reports';

const Settings = lazy(() => import('./Settings'));
const Approval = lazy(() => import('./Approvals'))


const PayRoll = lazy(() => import('./PayRoll'));
const PayRun = lazy(() => import('./PayRuns'));
const SelfService = lazy(() => import('./SelfService'));

const LeaveManagement = lazy(() => import('./LeaveManagement'));
const Reports = lazy(() => import('./Reports'));
const Coding = lazy(() => import('./Coding'))
const Transcription = lazy(() => import('./Transcription'))
const Organization = lazy(() => import('./Organization'))
const HelpDesk = lazy(() => import('./Helpdesk'))
const OnlineTest = lazy(() => import('./OnlineTest'))
const Domain = (props) => {

  return (
    <Fragment>
      <Suspense
        fallback={
          <div className="loader-container">
            <div className="loader-container-inner">
              <div className="text-center">
                <Loader type="line-scale-party" />
              </div>
              <h6 className="mt-3">Please wait while we load the Page</h6>
            </div>
          </div>
        }
      >
        <Route path={`${props.match.path}/Approval`} component={Approval} />
        <Route path={`${props.match.path}/Settings`} component={Settings} />
      </Suspense>

      <Suspense
        fallback={
          <div className="loader-container">
            <div className="loader-container-inner">
              <div className="text-center">
                <Loader type="line-scale-party" />
              </div>
              <h6 className="mt-3">Please wait while we load the Page</h6>
            </div>
          </div>
        }
      >

      </Suspense>

      {/* <Suspense fallback={
                <div className="loader-container">
                    <div className="loader-container-inner">
                        <div className="text-center">
                            <Loader type="line-scale-party" />
                        </div>
                        <h6 className="mt-3">
                            Please wait while we load the Page
                        </h6>
                    </div>
                </div>
            }>
                <Route path={`${props.match.path}/OP`} component={OP} />
            </Suspense> */}

      <Suspense
        fallback={
          <div className="loader-container">
            <div className="loader-container-inner">
              <div className="text-center">
                <Loader type="line-scale-party" />
              </div>
              <h6 className="mt-3">Please wait while we load the Page</h6>
            </div>
          </div>
        }
      >

        {/* <Route path={`${props.match.path}/Settings`} component={Settings} />
        <Route path={`${props.match.path}/Approval`} component={Approval}/> */}

        <Route path={`${props.match.path}/PayRoll`} component={PayRoll} />
        <Route path={`${props.match.path}/Leave`} component={LeaveManagement} />

      </Suspense>

      <Suspense
        fallback={
          <div className="loader-container">
            <div className="loader-container-inner">
              <div className="text-center">
                <Loader type="line-scale-party" />
              </div>
              <h6 className="mt-3">Please wait while we load the Page</h6>
            </div>
          </div>
        }
      >
        <Route path={`${props.match.path}/OnlineTest`} component={OnlineTest} />

      </Suspense>



      <Suspense
        fallback={
          <div className="loader-container">
            <div className="loader-container-inner">
              <div className="text-center">
                <Loader type="line-scale-party" />
              </div>
              <h6 className="mt-3">Please wait while we load the Page</h6>
            </div>
          </div>
        }
      >
        <Route path={`${props.match.path}/PayRun`} component={PayRun} />

      </Suspense>
      <Suspense
        fallback={
          <div className="loader-container">
            <div className="loader-container-inner">
              <div className="text-center">
                <Loader type="line-scale-party" />
              </div>
              <h6 className="mt-3">Please wait while we load the Page</h6>
            </div>
          </div>
        }
      >
        <Route path={`${props.match.path}/SelfService`} component={SelfService} />

      </Suspense>
      <Suspense
        fallback={
          <div className="loader-container">
            <div className="loader-container-inner">
              <div className="text-center">
                <Loader type="line-scale-party" />
              </div>
              <h6 className="mt-3">Please wait while we load the Page</h6>
            </div>
          </div>
        }
      >
        <Route path={`${props.match.path}/Reports`} component={Reports} />
        <Route path={`${props.match.path}/Coding`} component={Coding} />
        <Route path={`${props.match.path}/Transcription`} component={Transcription} />
        <Route path={`${props.match.path}/Organization`} component={Organization} />
        <Route path={`${props.match.path}/HelpDesk`} component={HelpDesk} />


      </Suspense>
    </Fragment>
  );
};

export default Domain;
