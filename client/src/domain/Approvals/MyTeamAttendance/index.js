import React ,{useState}from 'react';
import moment from "moment";
import {Card, CardBody} from 'reactstrap';
import PageHeader from "Layout/AppMain/PageHeader";
import ReportSearchComponent from 'domain/Reports/ReportsSearchComponent';
import {ReportingToAttendance} from 'domain/Reports/JSONNew/MappingReports/ReportingToAttendance';

export default function MyTeamAttendance(props) {
    //you can replace days with months and years.
  let lastWeekDate = moment(new Date(), "DD-MM-YYYY").subtract(8, 'days') // for previous day 
  let todayDate = moment(new Date(), "DD-MM-YYYY") // for today
  var date1 = moment(lastWeekDate).format("YYYY-MM-DD")
  var date2 = moment(todayDate).format("YYYY-MM-DD")
  const [filterValues, setFilterValues] = useState({ fromDate: date1, toDate: date2, attendanceStatus: '',employeeId:'' })
    return (
        <Card>
            <CardBody>
                <PageHeader title={"Attendance"} />
                <ReportSearchComponent filters={ReportingToAttendance.filters}
                    json={ReportingToAttendance} location={props.location} module={"LMS"} reportName={"Attendance"}
                    filterValues={filterValues} setFilterValues={setFilterValues}
                />
            </CardBody>
        </Card>
    )
}