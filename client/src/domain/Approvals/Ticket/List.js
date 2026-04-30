
// AS PER DISCUSSION ,WE CONSIDERED SCREEN IS NOT REQUIRED


// import React from 'react';
// import { Button, Card, CardBody, Col, Input, InputGroup, Row } from 'reactstrap';
// import DataTable from '../../../components/table/DataTable';
// import TicketService from '../../../services/Approval/TicketService';
// import UpdateTable from '../../HOC/withDataTable';
// import { StatusValues } from './Constvalues';

// const title = 'Tickets'
// const onEdit = true;
// const type = "Ticket"
// const fields = [{ name: 'raisedBy', label: 'Employee', type: 'select' },
// { name: 'message', label: 'Ticket Title', type: 'input' },
// { name: 'status', label: 'Status', type: 'select' },
// { name: 'action', label: 'Action', type: 'button' }
// ];

// const ddData = { status: { entity: 'status', data: [] } };
// ddData['status'].data = StatusValues;

// function TicketList(props) {

//     const searchCard = () => {
//         return (
//             <Card className="main-card mb-2">
//                 <CardBody>
//                     <Row form className="w-100">
//                         <Col md="3">
//                             <InputGroup>
//                                 <label className="text-capitalize mr-2" htmlFor='name'>Search Name</label>
//                                 <Input name="name" className="form-control form-control-sm"
//                                     value={props.searchData['name']}
//                                     onChange={(e) => props.handleOnChange(e.target.name, e.target.value)} />
//                             </InputGroup>
//                         </Col>
//                         <Col md="3">
//                             <Button className="mr-2  btn-icon btn-icon-only btn-secondary btn-sm" onClick={(e) => props.handleSearch(0)}>
//                                 <i className="pe-7s-search btn-icon-wrapper"> </i></Button>
//                         </Col>
//                     </Row>
//                 </CardBody>
//             </Card>
//         );
//     }
//     return (
//         <DataTable {...props} searchCard={searchCard} />
//     )
// }
// export default UpdateTable(TicketList, { ...TicketService.paginate() }, fields, ddData, title, onEdit, '', type)