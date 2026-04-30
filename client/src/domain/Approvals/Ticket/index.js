
// AS PER DISCUSSION ,WE CONSIDERED SCREEN IS NOT REQUIRED


// import { RWDropdownList, TextAreaInput } from 'components/dynamicform/Controls';
// import { Form, Formik } from 'formik';
// import PageHeader from 'Layout/AppMain/PageHeader';
// import React, { Fragment, useEffect, useState } from 'react';
// import ReactCSSTransitionGroup from 'react-addons-css-transition-group';
// import { Loader } from 'react-bootstrap-typeahead';
// import { Slide, toast } from 'react-toastify';
// import { Button, Card, CardBody, Col, Container, Row } from 'reactstrap';
// import ApprovalCompensatoryService from '../../../services/Approval/CompensatoryDayService';
// import TicketService from '../../../services/Approval/TicketService';
// import WFHService from '../../../services/Approval/WFHService';
// function Ticket(props) {
//     const [isLoading, setIsLoading] = useState(false)
//     const [state, setState] = useState({ entityValues: {}, employees: [], reporting: [] })
//     // const parsed = queryString.parse(props.location.search);
//     // const rid = parsed.r ? crypto.decrypt(parsed.r) : null;

//     const rid = props.rid ? props.rid : null;
  
//     useEffect(() => {
//         setIsLoading(true)
//         const fetchData = async () => {
//             let entityValues = {}, employees = [], reporting = {}

//             await ApprovalCompensatoryService.GetEmployees().then((result) => {
//                 reporting = result.data;
//             })
//             if (rid) {

//                 await WFHService.getEmp(props.state.employeeId).then((result) => {
//                     employees = result.data;
//                 })
//                 setState({ ...state, entityValues: entityValues, employees: employees, reporting: reporting })
                
//             }
//             setIsLoading(false);
//         }
//         fetchData();
//     }, [])

//     let formValues = {
//         ticket: '',
//         category: '',
//         status: '',
//         description: '',
//         chooseFile: ''
//     }

//     const handleSave = async (values, actions) => {

//         const data = {
            
//             employeeId: props.state.employeeId,
//             tickettitle: props.state.ticketTitle,
//             category: props.state.category,
//             status: values.status,
//             description: values.description,
//         }
//         try {
//             if (rid) {
//                 data['id'] = rid
//             }
//             TicketService.updateAsync(data).then((res)=>{
//                 if(res.status==200){
//                     notifySaved();
//                    // props.history.push(`/m/Approval/ticket`);
        
//                 }
//             })
        
//         }
//         catch (error) {
//             notifyError(error.message);
//         }
//         props.toggle()
//     }
//     const notifyError = (msg) => toast(msg, {
//         transition: Slide,
//         closeButton: true,
//         autoClose: 2500,
//         position: 'bottom-center',
//         type: 'error'
//     });

//     const notifySaved = () => toast("Data saved successfully!", {
//         transition: Slide,
//         closeButton: true,
//         autoClose: 2500,
//         position: 'bottom-center',
//         type: 'success'
//     });
//     return (
//         isLoading ? <Loader type="ball-grid-pulse" /> :
//             <Fragment>
//                 <PageHeader title='Tickets'/>
//                 <ReactCSSTransitionGroup
//                     component="div"
//                     transitionName="TabsAnimation"
//                     transitionAppear={true}
//                     transitionAppearTimeout={0}
//                     transitionEnter={false}
//                     transitionLeave={false}
//                     key="roles"
//                 >
//                     <Container fluid>
//                         <Formik
//                             initialValues={formValues}
//                             //validationSchema={validationSchema}
//                             onSubmit={(values, actions) => handleSave(values, actions)}
//                         >
//                             {({ values, errors, touched, setFieldValue, actions }) => {
//                                 const handleValueChange = async (name, value, { selected }) => {
//                                     setFieldValue(name, value)
//                                 };
//                                 return (
//                                     <Form>
//                                         <Card className='mb-3'>
//                                             <CardBody>
//                                                 <Row>
//                                                     {/* <Col xs='6'>
//                                                         <RWDropdownList
//                                                             {...{
//                                                                 name: 'employee',
//                                                                 label: 'Employee',
//                                                                 valueField: 'id',
//                                                                 textField: 'employeeName',
//                                                                 value: values['employee'],
//                                                                 //values: state.organization,
//                                                                 error: errors['employee'],
//                                                                 touched: touched['employee'],
//                                                             }}
//                                                             handlevaluechange={handleValueChange}
//                                                         />
//                                                     </Col> */}
//                                                     {/* <Col xs='6'>
//                                                         <Input {...{
//                                                             name: 'ticket', label: 'Ticket Title', type: 'string',
//                                                             value: values['ticket'], values: ['ticket'],
//                                                             touched: touched['ticket'], error: errors['ticket']
//                                                         }} handlevaluechange={handleValueChange}
//                                                         />
//                                                     </Col>
//                                                     <Col xs='6'>
//                                                         <RWDropdownList
//                                                             {...{
//                                                                 name: 'category',
//                                                                 label: 'Category',
//                                                                 valueField: 'id',
//                                                                 textField: 'category',
//                                                                 value: values['category'],
//                                                                 //values: state.organization,
//                                                                 error: errors['category'],
//                                                                 touched: touched['category'],
//                                                             }}
//                                                             handlevaluechange={handleValueChange}
//                                                         />
//                                                     </Col> */}
//                                                     <Col xs='6'>
//                                                         <RWDropdownList
//                                                             {...{
//                                                                 name: 'status',
//                                                                 label: 'Status',
//                                                                 valueField: 'value',
//                                                                 textField: 'text',
//                                                                 value: values['status'],
//                                                                 values: [{ value: 1, text: 'Initiated' }, { value: 2, text: 'Processing' }, { value: 3, text: 'Rejected' }],
//                                                                 error: errors['status'],
//                                                                 touched: touched['status'],
//                                                             }}
//                                                             handlevaluechange={handleValueChange}
//                                                         />
//                                                     </Col>
//                                                     <Col md='6'>
//                                                         <TextAreaInput {...{
//                                                             name: 'description', label: 'description', value: values['description'], values: ['description'],
//                                                             error: errors['description'], touched: touched['description']
//                                                         }} handlevaluechange={handleValueChange}
//                                                         />
//                                                     </Col>
//                                                     {/* <Col md='6'>
//                                                         <Upload {...{
//                                                             name: 'chooseFile', label: 'Choose File', type: 'text',
//                                                             value: values['chooseFile'], error: errors['chooseFile'],
//                                                             touched: touched['chooseFile']
//                                                         }} handlevaluechange={handleValueChange} />
//                                                     </Col> */}
//                                                     {/* <Col xs='6'>
//                                                         <Radio {...{
//                                                             name: 'approve', label: 'Do You Want To Approve', disabled: false,
//                                                             value: values['approve'],
//                                                             values: [{ value: 1, text: 'Yes' }, { value: 2, text: 'No' }],
//                                                             touched: touched['approve'],
//                                                             error: errors['approve']
//                                                         }} handlevaluechange={handleValueChange}
//                                                         />
//                                                     </Col>
//                                                     {values.approve == 2 ?
//                                                         <Col md='6'>
//                                                             <TextAreaInput {...{
//                                                                 name: 'reason', label: 'reason', value: values['reason'], values: ['reason'],
//                                                                 error: errors['reason'], touched: touched['reason']
//                                                             }} handlevaluechange={handleValueChange}
//                                                             />
//                                                         </Col> : ''
//                                                     } */}
//                                                 </Row>
//                                             </CardBody>
//                                         </Card>
//                                         <Row className='justify-content-center'>
//                                             <Button className="mb-2 mr-2 btn-icon btn-success" key='button' color="success" type="submit" name="save" >
//                                                 <i className="pe-7s-paper-plane btn-icon-wrapper font-weight-bold text-black"> </i> {"Save"}
//                                             </Button>
//                                         </Row>
//                                     </Form>
//                                 )
//                             }}
//                         </Formik>
//                     </Container>
//                 </ReactCSSTransitionGroup>
//             </Fragment>
//     );
// }
// export default Ticket