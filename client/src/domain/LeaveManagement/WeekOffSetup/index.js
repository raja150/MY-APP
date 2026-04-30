import { Input, SwitchInput } from 'components/dynamicform/Controls';
import { Form, Formik } from 'formik';
import PageHeader from 'Layout/AppMain/PageHeader';
import queryString from 'query-string';
import React, { Fragment, useEffect, useState } from 'react';
import Tabs, { TabPane } from 'rc-tabs';
import { Button, Card, CardBody, Col, Container, Row } from 'reactstrap';
import WeekOffSetupService from 'services/Leave/WeekOffSetup';
import * as crypto from 'utils/Crypto';
import * as formUtil from 'utils/form';
import { notifySaved } from 'components/alert/Toast';
import { WeekOffTabs } from './ConstValues';
import Loading from 'components/Loader';
import TabContent from 'rc-tabs/lib/TabContent';
import ScrollableInkTabBar from 'rc-tabs/lib/ScrollableInkTabBar';
import * as compare from 'utils/Compare';
import EmployeeWeekOffList from './EmployeeWeekOffList';
import AddEmployeeWeekOff from './AddEmployeeWeekOff';
import DepartmentWeekOffList from './DepartmentWeekOffList';
import AddDepartmentWeekOff from './AddDepartmentWeekOff';
import AddDesignationWeekOff from './AddDesignationWeekOff';
import DesignationWeekOffList from './DesignationWeekOffList';
import LocationWeekOffList from './LocationWeekOffList';
import AddLocationWeekOff from './AddLocationWeekOff';
import AddTeamWeekOff from './AddTeamWeekOff';
import TeamWeekOffList from './TeamWeekOffList';
import WeekOffDaysList from './WeekOffDaysList';
import AddWeekOffDays from './AddWeekOffDays';


function WeekOffSetupForm(props) {
    const [state, setState] = useState({ entityValues: {}, isLoading: true, employees: [], reporting: [], formValues: {}, })
    const [results, setResults] = useState({ returnResults: {} })
    const parsed = queryString.parse(props.location.search);
    const rid = parsed.r ? crypto.decrypt(parsed.r) : null;

    const [add, setAdd] = useState(true)
    const [deptAdd, setDeptAdd] = useState(false)
    const [designAdd, setDesignAdd] = useState(false)
    const [locAdd, setLocAdd] = useState(false)
//  const [locAllocationId, setLocAllocationId] = useState(null)
    const [teamAdd, setTeamAdd] = useState(false)
    const [weekOffAdd, setWeekOffAdd] = useState(false)

    useEffect(() => {
        const fetchData = async () => {
            let formValues = {}
            if (rid) {
                await WeekOffSetupService.getById(rid).then((res) => {
                    formValues = res.data;
                })
            }
            else {
                formValues = {
                    name: '',
                    status: ''
                }
            }
            setState({ ...state, formValues: formValues, isLoading: false })
        }
        fetchData();
    }, [])
    const handleSubmit = async (values, actions) => {
        let returnResults = {}
        const data = values;
        if (rid) {
            data['id'] = rid;
        }
        await WeekOffSetupService.UpdateAsync(data).then((res) => {
            notifySaved();
            returnResults = res.data;
        }).catch((err) => {
            formUtil.displayFormikError(err, actions)
        })
        setResults({ ...results, returnResults: returnResults })
    }
    const handleAddEmp = async (id) => {
        setAdd(!add)
    }
    const handleAddDept = (deptId) => {
        setDeptAdd(!deptAdd)
    }

    const handleAddDesign = (designId) => {
        setDesignAdd(!designAdd)
    }

    const handleAddLoc = (locId) => {
        setLocAdd(!locAdd)
//        setLocAllocationId(locId)
    }
    const handleAddTeam = (teamId) => {
        setTeamAdd(!teamAdd)
    }

    const handleAddWeekOffDays = (weekOffId) => {
        setWeekOffAdd(!weekOffAdd)

    }
    return <Fragment>
        <Card>
            <CardBody>
                <PageHeader title={''} />
                {state.isLoading ? <Loading /> :
                    <Container fluid>
                        <Formik
                            initialValues={state.formValues}
                            onSubmit={(values, actions) => handleSubmit(values, actions)}
                        >
                            {({ values, errors, touched, setFieldValue, isSubmitting, setValues }) => {
                                const handleValueChange = async (name, value, { selected }) => {
                                    setFieldValue(name, value)
                                };
                                return <Form>
                                    <Row>
                                        <Col xs='6'>
                                            <Input {...{
                                                name: 'name', label: 'Name', value: values['name'],
                                                error: errors['name'], touched: touched['name']
                                            }} handlevaluechange={handleValueChange}
                                            />
                                        </Col>
                                        <Col md='6'>
                                            <SwitchInput {...{
                                                name: `status`, label: 'Status',
                                                value: values['status'], touched: touched['status'], error: errors['status'],
                                                values: [{ value: true, text: 'Enable' }, { value: false, text: "Disable" }]
                                            }}
                                                handlevaluechange={handleValueChange}
                                            />
                                        </Col>
                                    </Row>
                                    <div className='d-flex'>
                                        <Button className="mb-2 mr-2 btn-icon btn-success" key='button' color="success" type="submit" name="save" handlesubmit={handleSubmit}>
                                            <i className="pe-7s-paper-plane btn-icon-wrapper font-weight-bold text-black"> </i> {"Save"}
                                        </Button>
                                    </div>

                                    <Row>
                                        {results.returnResults.id || rid ?
                                            <Tabs defaultActiveKey="0"
                                                renderTabBar={() => <ScrollableInkTabBar />}
                                                renderTabContent={() => <TabContent />}
                                            >
                                                {
                                                    WeekOffTabs.length > 0 && WeekOffTabs.map((tab, key) => {
                                                        return (
                                                            <TabPane tab={tab.text} key={key}>
                                                                {
                                                                    compare.isEqual(key, 0) && weekOffAdd ? <AddWeekOffDays {...props}  handleAddWeekOffDays={handleAddWeekOffDays} refId={results.returnResults && results.returnResults.id} />
                                                                        : compare.isEqual(key, 0) && <WeekOffDaysList {...props} refId={rid ? rid : results.returnResults.id} handleAddWeekOffDays={handleAddWeekOffDays} />
                                                                }
                                                                {
                                                                    compare.isEqual(key, 1) && locAdd ? <AddLocationWeekOff {...props} handleAddLoc={handleAddLoc} /> :
                                                                        compare.isEqual(key, 1) && <LocationWeekOffList {...props} refId={rid ? rid : results.returnResults.id} handleAddLoc={handleAddLoc} />

                                                                }
                                                                {
                                                                    compare.isEqual(key, 2) && deptAdd ? <AddDepartmentWeekOff {...props} handleAddDept={handleAddDept}  /> :
                                                                        compare.isEqual(key, 2) && <DepartmentWeekOffList {...props} refId={rid ? rid : results.returnResults.id} handleAddDept={handleAddDept} />

                                                                }
                                                                {
                                                                    compare.isEqual(key, 3) && designAdd ? <AddDesignationWeekOff {...props} handleAddDesign={handleAddDesign} /> :
                                                                        compare.isEqual(key, 3) && <DesignationWeekOffList {...props} refId={rid ? rid : results.returnResults.id} handleAddDesign={handleAddDesign} />

                                                                }
                                                                {
                                                                    compare.isEqual(key, 4) && teamAdd ? <AddTeamWeekOff {...props} handleAddTeam={handleAddTeam}  /> :
                                                                        compare.isEqual(key, 4) && <TeamWeekOffList {...props} refId={rid ? rid : results.returnResults.id} handleAddTeam={handleAddTeam} />

                                                                }

                                                                {
                                                                    compare.isEqual(key, 5) && add ? <EmployeeWeekOffList {...props} refId={rid ? rid : results.returnResults.id} handleAddEmp={handleAddEmp} />
                                                                        : compare.isEqual(key, 5) && <AddEmployeeWeekOff {...props}  handleAddEmp={handleAddEmp} />
                                                                }
                                                            </TabPane>
                                                        )
                                                    })
                                                }
                                            </Tabs>
                                            : ''}
                                    </Row>
                                </Form>
                            }}
                        </Formik>
                    </Container>
                }
            </CardBody>
        </Card>
    </Fragment>

}
export default WeekOffSetupForm;