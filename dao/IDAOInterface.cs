using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficViolationApp.dao
{
    public interface IDAOInterface<T, ID>
    {
        public int insert(T t);
        public int update(T t);
        public int delete(T t);
        public List<T> selectAll();
        public T selectById(ID id);
    }
}
