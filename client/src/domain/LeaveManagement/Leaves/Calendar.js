import Loading from 'components/Loader';
import moment from 'moment';
import React, { Fragment, useEffect, useState, useCallback } from 'react';
import { Calendar, momentLocalizer, Views } from 'react-big-calendar';
import { Card, CardBody, Col, Row } from 'reactstrap';
import { LEAVE_STATUS } from 'Site_constants';
import ApplyLeavesService from 'services/Leave/ApplyLeave';

const MyCalendar = (props) => {
  const [state, setState] = useState({ events: [], isLoading: true });
  const [date, setDate] = useState(new Date())

  const fetchData = (data) => {
    const events = data.map((g, k) => {
      let sts = LEAVE_STATUS.filter((i) => i.value == g.status);
      sts = sts[0] || { value: 0, text: '-', color: 'green', bgcolor: 'red' }
      return ({
        id: g.id,
        title: `${g.employeeName}`,
        start: new Date(g.fromDate),
        end: new Date(g.toDate).setDate(new Date(g.toDate).getDate(new Date(g.toDate)) + 1),
        color: g.status == 1 ? 'green' : 'blue',
      })
    })
    setState({ isLoading: false, events: events });
  }

  useEffect(() => {
    fetchData(props.data);
  }, [props]);

  const localizer = momentLocalizer(moment)
  const [calenderView, setCalenderView] = useState(Views.MONTH);
  const [calendarMinMax, setCalendarMinMax] = useState({ minDt: moment(new Date(2022, 10, 0, 14, 0, 0)), maxDt: moment(new Date(2017, 10, 0, 22, 0, 0)) });

  const handleRangeChange = async (e) => {
    let min = e.start
    let max = e.end
    var fromDate = moment(min).format(moment.HTML5_FMT.DATE);
    var toDate = moment(max).format(moment.HTML5_FMT.DATE);

    await ApplyLeavesService.GetLeavesBetweenDates(fromDate, toDate).then((response) => {
      fetchData(response.data)
    });
  }
  const onNavigate = useCallback((newDate) => setDate(newDate), [setDate]);
  return (
    state.isLoading ? <Loading /> :
      <Fragment>
        <Card>
          <CardBody>
            <Row className='mb-4'>
              <Col md='12'>
                Employees who are in leave in the Month of: <b>{date.toLocaleString('default', { month: 'long' })}</b>
              </Col>
            </Row>
            <Row>
              <Col md='12'>
                <Calendar
                  date={date}
                  localizer={localizer}
                  views={[Views.MONTH]}
                  events={state.events}
                  eventPropGetter={(event, start, end, isSelected) => ({
                    event,
                    start,
                    end,
                    isSelected,
                    style: { backgroundColor: "green" }
                  })}
                  onView={(event) => setCalenderView(event)}
                  min={new Date(calendarMinMax.minDt)}
                  max={new Date(calendarMinMax.maxDt)}
                  onNavigate={onNavigate}
                  onRangeChange={handleRangeChange}
                  popup
                  startAccessor="start"
                  endAccessor="end"
                  style={{ height: 700 }} 
                />
              </Col>

            </Row>
          </CardBody>
        </Card>

      </Fragment>
  )
}

export default MyCalendar;