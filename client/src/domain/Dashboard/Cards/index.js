import React, { Fragment } from 'react';
import 'react-widgets/dist/css/react-widgets.css';
import { Col, Row } from 'reactstrap';
import Appointments from './Appointments';
import PatientGender from './PatientGender';
import Revenue from './Revenue';
import SingleNumber from './SingleNumber';
import Trending from './Trending';







const data55 = [
    { name: 'Page A', uv: 4000, pv: 2400, amt: 2400 },
    { name: 'Page B', uv: 3000, pv: 1398, amt: 2210 },
    { name: 'Page C', uv: 2000, pv: 9800, amt: 2290 },
    { name: 'Page D', uv: 2780, pv: 3908, amt: 2000 },
    { name: 'Page E', uv: 1890, pv: 4800, amt: 2181 },
    { name: 'Page F', uv: 2390, pv: 3800, amt: 2500 },
    { name: 'Page G', uv: 3490, pv: 4300, amt: 2100 },
    { name: 'Page C', uv: 2000, pv: 6800, amt: 2290 },
    { name: 'Page D', uv: 4780, pv: 7908, amt: 2000 },
    { name: 'Page E', uv: 2890, pv: 9800, amt: 2181 },
    { name: 'Page F', uv: 1390, pv: 3800, amt: 1500 },
    { name: 'Page G', uv: 3490, pv: 4300, amt: 2100 },
];

const data4 = {
    labels: ['Eating', 'Drinking', 'Sleeping', 'Designing', 'Coding', 'Cycling', 'Running'],
    datasets: [
        {
            label: 'My First dataset',
            backgroundColor: 'rgba(179,181,198,0.2)',
            borderColor: 'rgba(179,181,198,1)',
            pointBackgroundColor: 'rgba(179,181,198,1)',
            pointBorderColor: '#fff',
            pointHoverBackgroundColor: '#fff',
            pointHoverBorderColor: 'rgba(179,181,198,1)',
            data: [65, 59, 90, 81, 56, 55, 40]
        },
        {
            label: 'My Second dataset',
            backgroundColor: 'rgba(255,99,132,0.2)',
            borderColor: 'rgba(255,99,132,1)',
            pointBackgroundColor: 'rgba(255,99,132,1)',
            pointBorderColor: '#fff',
            pointHoverBackgroundColor: '#fff',
            pointHoverBorderColor: 'rgba(255,99,132,1)',
            data: [28, 48, 40, 19, 96, 27, 100]
        }
    ]
};

const Cards = (props) => {

    const getCardComponent = (type) => {
        switch (type) {
            case 1:
                return <PatientGender data={props.apiData} type="1" header="Current No of Patient" />
            case 2:
                return <PatientGender data={props.apiData} type="2" header="Patient Registered" />
            case 3:
                return <PatientGender data={props.apiData} type="3" header="Upcoming Followup" />
            case 4:
                return <Revenue data={props.apiData} type="4" header="Revenue" />
            case 5:
                return <Trending data={props.apiData} type="5" header="Top Trending Symptoms" />
            case 6:
                return <Trending data={props.apiData} type="6" header="Fast moving products" />
            case 7:
                return <Trending data={props.apiData} type="7" header="Least moving products" />
            case 8:
                return <SingleNumber data={props.apiData} type="8" header="Product Expiry Alerts" label="Product Count" />
            case 9:
                return <SingleNumber data={props.apiData} type="9" header="Expense" label="Amount" />
            case 10:
                return <Appointments data={props.apiData} />
            case 12:
                return <SingleNumber data={props.apiData} type="12" header="Pharmacy Product Count" label="Total" />
            case 13:
                return
        }
    }
    return (
        <Fragment>
            {props.cardTypes && props.cardTypes.map((types, i) => {
                return <Row className="mb-2" key={`cardrow${i}`}>
                    {
                        types.map((item, j) => {
                            return <Col md={item.cardsize} key={`cardcol${j}`}>
                                {getCardComponent(parseInt(item.type))}
                            </Col>
                        })
                    }
                </Row>
                
            })}            
        </Fragment>
    )

}

export default Cards;
