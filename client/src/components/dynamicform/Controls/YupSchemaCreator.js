import * as yup from "yup";

export function createYupSchema(schema, config) {

  const { name, jsType, required, componentType, auto, validationRule, min, max } = config;
  // const validationType = (type === "checkbox" || type === "radio" ? "boolean" : "string");
  const validationType = (jsType === "number" ? "number" : jsType === "date" ? "date" : "string");
  if (!yup[validationType]) {
    return schema;
  }
  let validator = yup[validationType]();

  if (required && (auto || false) === false) {
    validator = validator["required"](`${config.label} is required!`);
    // validator = validator["nullable"];
  } else if (config.conditional) {
    //conditional fields 
    if (config.conditional.required) {
      
      validator = validator["when"](config.conditional.field, {
        is: (val, schema) => { return val == config.conditional.hasValue; },
        then: validator["required"](config.label ? `${config.label} is required!` : `Required!`)
      }); 
    }
  } else {
    validator = validator["nullable"](true);
  }

  if (jsType === "number" || jsType === "decimal") {
    // validator = validator["positive"](`${config.label} must be a positive`);
    validator = validator["typeError"](`${config.label} must be a number`)
  }
  // if (dataType === "checkbox") {
  //   validator = validator["oneOf"](["true"], 'You must accept the terms and conditions.');
  // }

  if (validationRule && validationRule.length > 0) {
    let rules = JSON.parse(validationRule);
    rules.map((rule) => {
      validator = validator["when"](rule.field, {
        is: (val) => val == rule.value,
        then: validator[rule.type](rule.params)
      });
    })
  }

  if (min != undefined && (min > 0 || min === 0)) {
    validator = validator["min"](min, (jsType === "number" ? `${config.label} must be greater than or equal to ${min}` : `${config.label} must be at least ${min} characters`));
  }
  if (max != undefined && max >= 0) {
    validator = validator["max"](max, (jsType === "number" ? `${config.label} must be less than or equal to ${max}` : `${config.label} must be at most ${max} characters`));
  }

  if (componentType === "pannumber") {
    const panRegExp = /^([a-zA-Z]){5}([0-9]){4}([a-zA-Z]){1}?$/;
    validator = validator["matches"](panRegExp, 'Invalid PAN number');
  }
  else if (componentType === "aadhaar") {
    const panRegExp = /^\d{12}$/;
    validator = validator["matches"](panRegExp, 'Invalid AADHAR number');
  }
  else if (componentType === "pincode") {
    const panRegExp = /^[1-9][0-9]{5}$/;
    validator = validator["matches"](panRegExp, 'Invalid Pincode number');
  }
  else if (componentType === "email") {
    const panRegExp = /^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5})$/;
    validator = validator["matches"](panRegExp, 'Invalid Email Address');
  }
  schema[name] = validator;
  return schema;
}
