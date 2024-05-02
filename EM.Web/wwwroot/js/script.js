function formatarCPF(cpf) {
    cpf = cpf.replace(/\D/g, ''); 
    cpf = cpf.replace(/^(\d{3})(\d)/, '$1.$2');
    cpf = cpf.replace(/^(\d{3})\.(\d{3})(\d)/, '$1.$2.$3'); 
    cpf = cpf.replace(/^(\d{3})\.(\d{3})\.(\d{3})(\d)/, '$1.$2.$3-$4'); 
    return cpf;
}

document.getElementById('inputCPF').addEventListener('input', function () {
    var cpf = this.value;
    this.value = formatarCPF(cpf);
});
