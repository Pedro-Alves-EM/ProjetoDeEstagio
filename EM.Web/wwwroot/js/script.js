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

document.getElementById('inputSearch').addEventListener('change', function () {
    var selectedOption = this.value;
    var formAction = '';

    switch (selectedOption) {
        case 'matricula':
            formAction = '@Url.Action("AchePorMatricula", "Aluno")';
            break;
        case 'nome':
            formAction = '@Url.Action("AchePorNome", "Aluno")';
            break;

        case 'uf':
            formAction = '@Url.Action("AchePorUf", "Aluno")';
            break;
        default:
            formAction = ''; // Ação padrão caso nenhuma opção seja selecionada
            break;
    }

    document.getElementById('searchForm').action = formAction;
});



