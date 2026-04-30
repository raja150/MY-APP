import Loading from 'components/Loader';
import React, { useEffect, useState } from 'react';
import { Card, CardBody, CardHeader, Col, Row } from 'reactstrap';
import { GENDER, MARITAL_STATUS } from 'Site_constants';
import * as dateUtil from 'utils/date';
import ProfileEmergencyAddress from './ProfileEmergencyAddress';
import ProfileInfoDisplay from './ProfileInfoDisplay';
import ProfilePermanentAddress from './ProfilePermanentAddress';
import ProfilePresentAddress from './ProfilePresentAddress';
import * as _ from 'lodash'
import PersonalInfoEdit from './PersonalInfoEdit';
import ProfileEdit from './ProfileEdit';
import * as formUtil from 'utils/form';
import { notifySaved } from 'components/alert/Toast';
import ImageService from 'services/ImageService';
import avatar from 'assets/utils/images/avatars/1.jpg';
import ContactList from './ContactList';
import ProfileService from 'services/Org/ProfileService';
import SessionStorageService from 'services/SessionStorage';
export default function Details() {

    const [state, setState] = useState({
        isLoading: true, empDetails: {}, contactDetails: [],
        resignationDetails: {}, presentAddress: {}, permanentAddress: {}, emergencyAddress: {}, callComponent: false
    })

    useEffect(() => {
        const fetchData = async () => {
            let empDetails = {}, contactDetails = [], resignationDetails = {}, presentAddress = {}, permanentAddress = {}, emergencyAddress = {}

            await ProfileService.getEmployeeDetails().then((response) => {
                empDetails = response.data
            })
            await ProfileService.getContact().then((response) => {
                contactDetails = response.data
            })
            await ProfileService.getPresentAddress().then((response) => {
                presentAddress = response.data
            })
            await ProfileService.getPermanentAddress().then((response) => {
                permanentAddress = response.data
            })
            await ProfileService.getEmergencyAddress().then((response) => {
                emergencyAddress = response.data
            })
            setState({
                ...state, isLoading: false, empDetails: empDetails, contactDetails: contactDetails,
                resignationDetails: resignationDetails, presentAddress: presentAddress, permanentAddress: permanentAddress, emergencyAddress: emergencyAddress
            })
        }
        fetchData();
    }, [])


    const [personalInfoModal, setPersonalInfoModal] = useState(false)
    const personalInfoToggle = () => { setPersonalInfoModal(!personalInfoModal) }

    const [profileModel, setProfileModel] = useState(false)
    const profileToggle = () => { setProfileModel(!profileModel) }

    const [presentAddModal, setPresentAddModal] = useState(false)
    const presentAddToggle = () => { setPresentAddModal(!presentAddModal) }

    const [permanentAddModel, setPermanentAddModel] = useState(false)
    const permanentAddModelToggle = () => {
        setPermanentAddModel(false)
    }

    const [emergencyModel, setEmergencyModel] = useState(false)
    const emergencyToggle = () => { setEmergencyModel(!emergencyModel) }

    const info = SessionStorageService.getUserInfo();
    let infoJson = {};
    if (info) {
        infoJson = JSON.parse(info);
    }
    const onChangeFile = async (event) => {

        const formData = new FormData();
        formData.append('employeeId', state.empDetails.id)
        formData.append('imageName', state.empDetails.no)
        formData.append('file', event.target.files[0])
        ImageService.PostAsync(formData).then((res) => {
            notifySaved();
            window.location.reload();
        }).catch((err) => {
            formUtil.displayErrors(err);
        })

    }

    const updateState = (name, value) => {
        setState({ ...state, [name]: value });
    }

    return (
        state.isLoading ? <Loading /> :
            <>
                <CardBody className="p-0 mt-2">
                    <Card className="mb-2">
                        <CardHeader className="card-header-tab">
                            <i className="header-icon lnr-user icon-gradient bg-ripe-malin" />
                            Profile
                        </CardHeader>
                        {state.empDetails ? <CardBody className="p-2">
                            <Row style={{ marginLeft: '3px' }}>
                                <Col md='3'>
                                    <Card className="main-card mb-3" style={{ height: "49%" }}>
                                        <CardBody >
                                            <center>
                                                <div>
                                                    <img htmlFor="file" style={{ borderRadius: '50%', width: 100, height: 100 }} className="big cc visa icon"
                                                        src={`${process.env.REACT_APP_IMAGE_ENDPOINT}/original/${infoJson.image}.jpg`}
                                                        onError={(e) => (e.target.onerror = null, e.target.src = avatar)}
                                                    />
                                                    {/* <div>
                                                        <label htmlFor="file"><span className="lnr lnr-camera" style={{ fontSize: '20px' }}  > </span></label>
                                                        <input type="file" id="file" style={{ display: 'none' }} onChange={onChangeFile} />
                                                    </div> */}
                                                </div>
                                                <h6 ><b>
                                                    <div>
                                                        {state.empDetails.name} ({state.empDetails.no})
                                                        {/* <span className="lnr lnr-pencil" style={{ marginLeft: '5px' }} onClick={() => setProfileModel(true)} ></span> */}
                                                    </div>
                                                </b></h6>
                                                {state.empDetails.designation}
                                            </center>
                                        </CardBody>
                                    </Card>
                                    <Card className="main-card mb-5" style={{ height: "40%" }}>
                                        <CardHeader>
                                            Basic Info
                                        </CardHeader>
                                        <CardBody >
                                            <div className="btn-actions-pane-left">
                                                <span className="lnr lnr-calendar-full" style={{ marginRight: '10px', fontSize: '15px' }}></span>
                                                {state.empDetails && dateUtil.getDate(state.empDetails.dateOfJoining)}
                                            </div>

                                            <div className="btn-actions-pane-left">
                                                <span className="lnr lnr-apartment" style={{ marginRight: '10px', fontSize: '15px' }}></span>
                                                {state.empDetails && state.empDetails.departmentName}
                                            </div>

                                            <div className="btn-actions-pane-left">
                                                <span tooltip='Team' className="lnr lnr-users" style={{ marginRight: '12px', fontSize: '15px' }}></span>
                                                {state.empDetails && state.empDetails.employeeTeam || "-"}
                                            </div>

                                            <div className="btn-actions-pane-left">
                                                <span className="lnr lnr-envelope" style={{ marginRight: '12px', fontSize: '15px' }}></span>
                                                {state.empDetails && state.empDetails.workEmail || "-"}
                                            </div>

                                            <div className="btn-actions-pane-left">
                                                <span className="lnr lnr-map-marker" style={{ marginRight: '12px', fontSize: '15px' }}></span>
                                                {state.empDetails && state.empDetails.workLocation}
                                            </div>
                                        </CardBody>
                                    </Card>
                                </Col>
                                <Col md='9'>
                                    <Card className="main-card mb-3">
                                        <CardHeader>
                                            Personal Information
                                            <div className="btn-actions-pane-right" onClick={() => setPersonalInfoModal(true)}  >
                                                {/* <span className="lnr lnr-pencil" style={{ fontSize: '18px' }}></span> */}
                                            </div>
                                        </CardHeader>
                                        <CardBody >
                                            <ProfileInfoDisplay label="Employee DOB" info={dateUtil.getDate(state.empDetails.dateOfBirth)} />
                                            <ProfileInfoDisplay label="DOB As Per Certificate" info={dateUtil.getDate(state.empDetails.dobc)} />
                                            <ProfileInfoDisplay label="Father Name" info={state.empDetails.fatherName} />
                                            <ProfileInfoDisplay label="Employee Gender" info={GENDER[state.empDetails.gender]} />
                                            <ProfileInfoDisplay label="Marital Status" info={MARITAL_STATUS[state.empDetails.maritalStatus]} />

                                            <ProfileInfoDisplay label="Aadhaar Number" info={state.empDetails.aadhaarNumber} />
                                            <ProfileInfoDisplay label="PAN Number" info={state.empDetails.panNumber} />
                                            <ProfileInfoDisplay label="Passport Number" info={state.empDetails.passportNumber} />
                                            <ProfileInfoDisplay label="Personal Email" info={state.empDetails.personalEmail} />
                                            <ProfileInfoDisplay label="Employee MobileNo" info={state.empDetails.mobileNumber} />
                                        </CardBody>
                                    </Card>
                                    <Row>
                                        <Col md='4'>
                                            <Card className="main-card mb-3" style={{ height: "87%" }}>
                                                <CardHeader>
                                                    Present Address
                                                    <div className="btn-actions-pane-right" onClick={() => setPresentAddModal(true)}  >
                                                        {/* <span className="lnr lnr-pencil" style={{ fontSize: '18px' }}></span> */}
                                                    </div>
                                                </CardHeader>
                                                <CardBody >
                                                    {state.presentAddress ? <div>
                                                        {state.presentAddress.addressLineOne},<br />
                                                        {state.presentAddress.addressLineTwo},<br />
                                                        {state.presentAddress.cityOrTown},<br />
                                                        {state.presentAddress.state},<br />
                                                        {state.presentAddress.country}.
                                                    </div>
                                                        : <div>-</div>}
                                                </CardBody>
                                            </Card>
                                        </Col>
                                        <Col md='4'>
                                            <Card className="main-card mb-3" style={{ height: "87%" }}>
                                                <CardHeader>
                                                    Permanent Address
                                                    <div className="btn-actions-pane-right" onClick={() => setPermanentAddModel(true)}  >
                                                        {/* <span className="lnr lnr-pencil" style={{ fontSize: '18px' }}></span> */}
                                                    </div>
                                                </CardHeader>
                                                <CardBody >
                                                    {(state.permanentAddress && state.permanentAddress.sameAsPresent == 0) ?
                                                        <>
                                                            <div>
                                                                {state.permanentAddress.addressLineOne},<br />
                                                                {state.permanentAddress.addressLineTwo},<br />
                                                                {state.permanentAddress.cityOrTown},<br />
                                                                {state.permanentAddress.state},<br />
                                                                {state.permanentAddress.country}.
                                                            </div>
                                                        </> : <div>Same As Present Address</div>
                                                    }
                                                </CardBody>
                                            </Card>
                                        </Col>
                                        <Col md='4'>
                                            <Card className="main-card mb-3" style={{ height: "87%" }}>
                                                <CardHeader>
                                                    Emergency Address
                                                    <div className="btn-actions-pane-right" onClick={() => setEmergencyModel(true)}  >
                                                        {/* <span className="lnr lnr-pencil" style={{ fontSize: '18px' }}></span> */}
                                                    </div>
                                                </CardHeader>
                                                <CardBody >
                                                    {state.emergencyAddress ? <div>
                                                        {state.emergencyAddress.addressLineOne},<br />
                                                        {state.emergencyAddress.addressLineTwo},<br />
                                                        {state.emergencyAddress.cityOrTown},<br />
                                                        {state.emergencyAddress.state},<br />
                                                        {state.emergencyAddress.country},<br />
                                                        {state.emergencyAddress.emergencyConNo}.
                                                    </div> : <div>-</div>}
                                                </CardBody>
                                            </Card>
                                        </Col>
                                    </Row>
                                </Col>
                            </Row>
                            <Row style={{ marginLeft: '3px' }}>
                                <Col>
                                    <Card className="main-card mb-3">
                                        <CardBody >
                                            {<ContactList data={state.contactDetails} />}
                                        </CardBody>
                                    </Card>
                                </Col>
                            </Row>
                            {profileModel && <ProfileEdit model={profileModel} setProfileModel={setProfileModel} toggle={profileToggle} setProfileEditResult={updateState} data={state.empDetails} />}
                            {personalInfoModal && <PersonalInfoEdit modal={personalInfoModal} toggle={personalInfoToggle} setPersonalInfoResult={updateState} data={state.empDetails} />}
                            {presentAddModal && <ProfilePresentAddress modal={presentAddModal} toggle={presentAddToggle} setPresentResult={updateState} data={state.presentAddress} />}
                            {permanentAddModel && <ProfilePermanentAddress modal={permanentAddModel} toggle={permanentAddModelToggle} setPermanentResult={updateState} data={state.permanentAddress} presentAddress={state.presentAddress} />}
                            {emergencyModel && <ProfileEmergencyAddress modal={emergencyModel} toggle={emergencyToggle} setEmergencyResult={updateState} data={state.emergencyAddress} />}
                        </CardBody> : ''}
                    </Card>
                </CardBody>
            </>
    )
}
