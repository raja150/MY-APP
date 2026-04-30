import { notifyError } from "components/alert/Toast";
import React, { useEffect, useState } from 'react';
import Chart from 'react-apexcharts';
import PayMonthService from "services/PayRoll/PayMonth";

const Mixed = (props) => {
  return (
    <div className="bar">
      <Chart options={{
        chart: {
          id: "Annual Salary", height: 350, type: 'bar',
          toolbar: { show: false }
        },
        xaxis: {
          categories: props.categories
        }
      }} series={
        [
          {
            name: "Salary",
            data: props.data
          }
        ]
      } type="line" width="100%" height="325px" />
    </div>
  );
}

export default Mixed;
