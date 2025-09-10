function ValidaCep(value) {
  return value.match(/(\d{5})-(\d{3})/g) ? true : false;
}

function FormatCep(value) {
  if (value == null) return;

  return value
    .replace(/\D/g, "")
    .replace(/(\d{5})(\d)/, "$1-$2")
    .replace(/(-\d{3})\d+?$/, "$1");
}

function FormatCpf(value) {
  if (value == null) return;

  return value
    .replace(/\D/g, "")
    .replace(/(\d{3})(\d)/, "$1.$2")
    .replace(/(\d{3})(\d)/, "$1.$2")
    .replace(/(\d{3})(\d{1,2})$/, "$1-$2");
}

function FormatTelefone(value) {
  if (value == null) return;

  const numbers = value.replace(/\D/g, "");
  
  if (numbers.length <= 10) {
    return numbers
      .replace(/(\d{2})(\d)/, "($1) $2")
      .replace(/(\d{4})(\d)/, "$1-$2");
  } else {
    return numbers
      .replace(/(\d{2})(\d)/, "($1) $2")
      .replace(/(\d{5})(\d)/, "$1-$2");
  }
}

function CleanCpf(value) {
  if (value == null) return "";
  return value.replace(/\D/g, "");
}

function CleanTelefone(value) {
  if (value == null) return "";
  return value.replace(/\D/g, "");
}

function CleanCEP(value) {
  if (value == null) return "";
  return value.replace(/\D/g, "");
}

export { ValidaCep, FormatCep, FormatCpf, FormatTelefone, CleanCpf, CleanTelefone, CleanCEP };
