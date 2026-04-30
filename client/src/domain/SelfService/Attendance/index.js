import Loading from "components/Loader";
import { SelfAttendance } from "domain/Reports/JSONNew/MappingReports/SelfAttendance";
import ReportSearchComponent from "domain/Reports/ReportsSearchComponent";
import PageHeader from "Layout/AppMain/PageHeader";
import * as _ from "lodash";
import moment from "moment";
import React, { useEffect, useState } from "react";
import { Progress } from "react-sweet-progress";
import { Button, Card, CardBody, CardHeader, Col, Label, Modal, ModalBody, Row } from "reactstrap";
import AttendanceService from "services/LMAttendance/Attendance";
import * as dateUtil from "utils/date";
import WebAttendanceService from 'services/SelfService/WebAttendance';
import { notifyError, notifySaved } from 'components/alert/Toast'
import * as formUtil from 'utils/form';

function Attendance(props) {
  var today = new Date(),
    curTime = today.getHours(),
    curMin = today.getMinutes();
  const [state, setState] = useState({ isPunchIn: {}, isPunchedEmployee: '', isLoading: true, reportsData: [] });
  const [modal, setModal] = useState(false);
  const [fromDate, setFromDate] = useState(new Date());
  const [toDate, setToDate] = useState(new Date());

  //you can replace days with months and years.
  let lastWeekDate = moment(new Date(), "DD-MM-YYYY").subtract(8, 'days') // for previous day 
  let todayDate = moment(new Date(), "DD-MM-YYYY") // for today
  var date1 = moment(lastWeekDate).format("YYYY-MM-DD")
  var date2 = moment(todayDate).format("YYYY-MM-DD")
  const [filterValues, setFilterValues] = useState({ fromDate: date1, toDate: date2, attendanceStatus: '' })
  const fetchData = async () => {
    let isPunchIn = {}, isPunchedEmployee = '';
    await WebAttendanceService.getIsPunchInAsync().then(
      (res) => {
        isPunchIn = res.data;
      }
    );
    await WebAttendanceService.isPunchedEmployee().then((res) => {
      isPunchedEmployee = res.data
    })
    setState({ ...state, isLoading: false, isPunchIn: isPunchIn, isPunchedEmployee: isPunchedEmployee });
  };

  useEffect(() => {
    fetchData();
  }, []);


  const handleSubmitPunchOut = async () => {
    await WebAttendanceService.putAsync();
    fetchData();
  };

  const handleSubmit = async () => {
    const data = {};
    if (_.isEmpty(state.isPunchIn)) {
      await WebAttendanceService.postAsync(data).then((result) => {
        fetchData();
      }).catch((error) => {
        formUtil.displayErrors(error)
      });
    }
    // fetchData();
  };
  const handleSubmitRePunch = async () => {
    await WebAttendanceService.putRePunchIn();
    setModal(!modal);
    fetchData();
  };

  let date = !_.isEmpty(state.isPunchIn) ? new Date(state.isPunchIn.inTime) : new Date();
  //let date = new Date(!_.isEmpty(state.isPunchIn) && state.isPunchIn.inTime);
  let getting = date.getHours();
  let getting1 = date.getMinutes();
  const toggle = () => setModal(!modal);
  return state.isLoading ? (
    <Loading />
  ) : (
    <>
      <Row style={{ paddingLeft: "450px", width: "150%" }}>
        <Col md="3">
          {state.isPunchedEmployee ?
            <Card className="mb-3" md="6">
              <CardHeader className="card-header-tab" style={{ paddingLeft: "9px" }}>
                <div style={{ marginLeft: '80px', fontSize: "20px" }}>
                  Attendance
                </div>
              </CardHeader>
              <CardBody className="p-2">
                <div className="card-header-title font-size-lg text-capitalize font-weight-normal">
                  <h6>
                    <h5>Punch In At{" :    "}
                      {dateUtil.DisplayDateTime(!_.isEmpty(state.isPunchIn) && state.isPunchIn.inTime)}
                    </h5> </h6>
                </div>
                <div className="progress-circle-wrapper" style={{ padding: "20px", paddingLeft: "70px" }}>
                  <Progress
                    type="circle"
                    fontSize={15}
                    value="4"
                    percent={((curMin < getting1 ? curTime - getting - 1 : curTime - getting) / 8) * 100}
                    width="60%"
                    theme={{ active: { trailColor: "rgba(1,2,3,0.1)", color: "var(--success)", }, }}
                    style={{ fontSize: "30px" }}
                  />
                  <div style={{ fontSize: "30px", paddingLeft: "10px" }}>
                    {curMin < getting1 ? curTime - getting - 1 + (curTime - getting - 1 <= 1 ? " Hour" : " Hours") : curTime - getting + (curTime - getting - 1 <= 1 ? " Hour" : " Hours")}
                  </div>
                </div>
                {_.isEmpty(state.isPunchIn) ? (
                  <Button style={{ width: "70%", marginLeft: "45px", height: "50px", fontSize: "20px", fontFamily: "sans-serif", }}
                    className="mb-6 mr-6 btn-pill" color="success" onClick={handleSubmit}
                  >
                    Punch In
                  </Button>
                ) : !_.isEmpty(state.isPunchIn) && state.isPunchIn.outTime != null ? (
                  // <Button style={{ width: "70%", marginLeft: "45px", height: "50px", fontSize: "20px", fontFamily: "sans-serif", }}
                  //   className="mb-6 mr-6 btn-pill" color="success" onClick={() => setModal(!modal)}
                  // >
                  //   Re-Punch In
                  // </Button>
                  <Button style={{ width: "70%", marginLeft: "45px", height: "50px", fontSize: "20px", fontFamily: "sans-serif", }}
                    className="mb-6 mr-6 btn-pill" color="success" onClick={() => notifyError("you can't Punchin on the same day")}
                  >
                    Punch In
                  </Button>
                ) : (
                  <Button style={{ width: "70%", marginLeft: "45px", height: "50px", fontSize: "20px", fontFamily: "sans-serif", }}
                    className="mb-6 mr-6 btn-pill" color="success" onClick={handleSubmitPunchOut}
                  >
                    Punch Out
                  </Button>
                )}

                {/* <Modal isOpen={modal} toggle={toggle}>
                  <ModalBody>
                    <Col ms="4">
                      <Label><h6>Are you sure want to punch in again?</h6></Label>
                    </Col>
                    <Row>
                      <div style={{ paddingLeft: '30px' }} >
                        <Button type="button" color="success" onClick={() => handleSubmitRePunch()}>
                          Yes
                        </Button>

                      </div>
                      <div style={{ paddingLeft: '10px' }} >
                        <Button color="danger" type="button" onClick={() => setModal(!modal)} >
                          No
                        </Button>
                      </div>
                    </Row>
                  </ModalBody>
                </Modal> */}
                <Modal isOpen={modal} toggle={toggle}>
                  <ModalBody>
                    <Col ms="4">
                      <Label><h6> you can't Punchin on the same day</h6></Label>
                    </Col>
                  </ModalBody>
                </Modal>
              </CardBody>
            </Card>
            : ""}
        </Col>
      </Row>
      <Card>
        <CardBody>
          <PageHeader title={"Attendance"} />
          <ReportSearchComponent filters={SelfAttendance.filters}
            json={SelfAttendance} location={props.location} module={"LMS"} reportName={"Attendance"}
            filterValues={filterValues} setFilterValues={setFilterValues}
          />
        </CardBody>
      </Card>

    </>
  );
}
export default Attendance;