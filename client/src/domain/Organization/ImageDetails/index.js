import Loading from 'components/Loader';
import React, { useEffect, useState } from 'react';
import { Card, CardBody, Col, Label, Row } from 'reactstrap';
import { GENDER } from 'Site_constants';
import * as dateUtil from 'utils/date';
import * as _ from 'lodash'
import * as formUtil from 'utils/form';
import { notifySaved } from 'components/alert/Toast';
import ImageService from 'services/ImageService';
import EmployeeService from 'services/Org/Employee';
import * as crypto from 'utils/Crypto'
import queryString from 'query-string'
import ProfileInfoDisplay from 'Layout/Details/ProfileInfoDisplay';
import avatar from 'assets/utils/images/avatars/1.jpg';

export default function ImageDetails(props) {
    const [state, setState] = useState({ isLoading: true, empDetails: {} })
    const parsed = queryString.parse(props.location.search);
    const rid = parsed.r ? crypto.decrypt(parsed.r) : null;

    useEffect(() => {
        let empDetails = {}
        const fetchData = async () => {
            if (rid) {
                await EmployeeService.getEmployeeById(rid).then((response) => {
                    empDetails = response.data
                })
            }
            setState({ ...state, isLoading: false, empDetails: empDetails })
        }
        fetchData()
    }, [])

    const onChangeFile = async (event) => {

        const formData = new FormData();
        formData.append('employeeId', state.empDetails.id)
        formData.append('imageName', state.empDetails.no)
        formData.append('file', event.target.files[0])
        await ImageService.PostAsync(formData).then((res) => {
            notifySaved();
            window.location.reload();
        }).catch((err) => {
            formUtil.displayErrors(err);
        })
    }
    return (
        state.isLoading ? <Loading /> :
            <>
                <CardBody className="p-0 mt-2">
                    <Card className="mb-2">
                        {state.empDetails ? <CardBody className="p-2">
                            <Row >
                                <Col md='2'>
                                    <center>
                                        <div>
                                            <img htmlFor="file" style={{ borderRadius: '50%', width: 100, height: 100 }}
                                                className="big cc visa icon"
                                                src={`${process.env.REACT_APP_IMAGE_ENDPOINT}/avatars/${state.empDetails.id}.jpg`}
                                                onError={(e) => (e.target.onerror = null, e.target.src = avatar)} alt="" />
                                            <div>
                                                <label htmlFor="file"><span className="lnr lnr-camera" style={{ fontSize: '20px' }}  > </span></label>
                                                <input type="file" id="file" style={{ display: 'none' }} onChange={onChangeFile} />
                                            </div>
                                        </div>
                                    </center>
                                </Col>
                                <Col md='2'>
                                    <center>
                                        <img style={{ width: 150, height: 150 }} className="big cc visa icon"
                                            src={`${process.env.REACT_APP_IMAGE_ENDPOINT}/original/${state.empDetails.id}.jpg`}
                                            onError={(e) => (e.target.onerror = null, e.target.src = avatar)} alt="" />
                                        <h5>Original Image</h5>
                                    </center>
                                </Col>
                                <Col style={{ marginLeft: '30px' }}>
                                    <Label style={{ fontSize: '20px', marginBottom: '20px' }}>Employee Details</Label>

                                    <ProfileInfoDisplay label="Employee Name" info={state.empDetails.name} />
                                    <ProfileInfoDisplay label="Employee Code" info={state.empDetails.no} />
                                    <ProfileInfoDisplay label="Gender" info={GENDER[state.empDetails.gender]} />
                                    <ProfileInfoDisplay label="MobileNo" info={state.empDetails.mobileNumber} />
                                    <ProfileInfoDisplay label="Date of Joining" info={dateUtil.getDate(state.empDetails.dateOfJoining)} />
                                    <ProfileInfoDisplay label="Department" info={state.empDetails.departmentName} />
                                    <ProfileInfoDisplay label="Team" info={state.empDetails.employeeTeam} />
                                    <ProfileInfoDisplay label="Work Email" info={state.empDetails.workEmail} />
                                    <ProfileInfoDisplay label="Work Location" info={state.empDetails.workLocation} />
                                </Col>
                            </Row>
                        </CardBody> : ''}
                    </Card>
                </CardBody>
            </>
    )
}
