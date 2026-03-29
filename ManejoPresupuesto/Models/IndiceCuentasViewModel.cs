namespace ManejoPresupuesto.Models
{
    public class IndiceCuentasViewModel
    {
        public string TipoCuenta { get; set; }
        public IEnumerable<Cuenta> Cuentas { get; set; }

        //suma el balance de cada cuenta
        public decimal Balance => Cuentas.Sum(x => x.Balance);

        //decimal total = 0;
        //foreach (var cuenta in Cuentas)
        //{
        //    total += cuenta.Balance;
        //}
        //return total;
    }
}
