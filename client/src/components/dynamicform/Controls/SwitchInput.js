import BootstrapSwitchButton from 'bootstrap-switch-button-react';
import React from 'react';
import { FormGroup } from 'reactstrap';


const SwitchInput = (props) => {
  const { dpchange, handlevaluechange, value, label, touched, error, disabled, className, name, type, ...rest } = props;

  return (
    <FormGroup>
      <legend className="text-capitalize font-weight-normal" htmlFor={name}>{label}</legend>
      <BootstrapSwitchButton offstyle="danger"
        // disabled={disabled}
        checked={value === true ? true : false}
        onlabel={props.values && props.values[0] ? props.values[0].text : 'On'}
        onstyle='success'
        offlabel={props.values && props.values[1] ? props.values[1].text : 'Off'}
        style='rounded-pill w-25'
        onChange={(checked) => handlevaluechange(name, checked, {})}
      />

      {/* 
      <Switch
        onChange={handleChange}
        checked={field.value === true ? true : false}
        className="text-white"
        id="props.name"
        height={24}
        width={60}
        handleDiameter={20}
        uncheckedIcon={
          <div
            style={{
              display: "flex",
              justifyContent: "center",
              alignItems: "center",
              height: "100%",
              fontSize: 14,
              lineHeight: 0,
              paddingRight: 2
            }}
          >
            {props.values && props.values[1] ? props.values[1].text : 'Off'}
          </div>
        }
        checkedIcon={
          <div
            style={{
              display: "flex",
              justifyContent: "center",
              alignItems: "center",
              height: "100%",
              fontSize: 14,
              lineHeight: 0,
              color: "white",
              paddingRight: 2
            }}
          >
            {props.values && props.values[0] ? props.values[0].text : 'On'}
          </div>
        }
      /> */}
      {touched && error ?
        // <div className="invalid-feedback">
        //   {error}
        // </div>
        <div className="text-danger" style={{ fontSize: '11px' }}>
          {error}
        </div>
        : ''}
    </FormGroup>
  );
};

export default SwitchInput;
