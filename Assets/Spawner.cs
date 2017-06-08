using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public GameObject nodoPrefab;
    public Conexao conexaoPrefab;
    List<Nodo> nodos = new List<Nodo>();

    void Start () {
        CriarNodos();
	}

    void CriarNodos() {
        List<string> arquivoLido = new List<string>(LerAqruivo().Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
        List<int> numerosLidos = new List<int>();
        for (int i = 0; i < arquivoLido.Count; i++) {
            numerosLidos.Add(Int32.Parse(arquivoLido[i]));
        }


        
        for (int i = 0; i < numerosLidos.Count; i = i + 3) {
            //print(numerosLidos[i]);
            bool novo;
            GameObject nodo;
            if (GameObject.Find("Nodo " + numerosLidos[i].ToString()) == null) {
                nodo = Instantiate(nodoPrefab, transform.position, Quaternion.identity, transform);
                novo = true;
            }else{
                nodo = GameObject.Find("Nodo " + numerosLidos[i].ToString());
                novo = false;
            }

            Nodo nodoScript = nodo.GetComponent<Nodo>();
            
            nodoScript.numero = numerosLidos[i];
            nodoScript.gameObject.name = "Nodo " + numerosLidos[i].ToString();


            Conexao conexao = Instantiate(conexaoPrefab);
            conexao.transform.SetParent(nodo.transform);
            conexao.origem = numerosLidos[i];
            conexao.destino = numerosLidos[i + 1];
            conexao.custo = numerosLidos[i + 2];

            nodoScript.conexoes.Add(conexao);


            if (novo) {
                nodos.Add(nodoScript);
            }
            
        }

        print("numero de nodos na lista: " + nodos.Count);
        int numeroDeConexoes = 0;
        foreach (Nodo nodo in nodos) {
            foreach (Conexao conexao in nodo.conexoes) {
                numeroDeConexoes++;
            }
        }
        print(numeroDeConexoes);
        FlowNetwork flowNetwork = new FlowNetwork(numeroDeConexoes);

        foreach (Nodo nodo in nodos) {
            foreach (Conexao conexao in nodo.conexoes) {
                flowNetwork.AddEdge(new FlowEdge(nodo.numero, conexao.destino, conexao.custo));

            }
        }

        FordFulkerson fordFulkerson = new FordFulkerson(flowNetwork, 1, 99);

        print(fordFulkerson.Value);

        

    }

    
    string LerAqruivo() {

        string resultado = "";

        try {   // Open the text file using a stream reader.
            using (StreamReader sr = new StreamReader("C:\\Users\\renan\\Documents\\Unity Projects\\Fluxo Maximo\\Assets\\arquivo.txt")) {
                // Read the stream to a string, and write the string to the console.
                resultado = sr.ReadToEnd();
            }
        } catch (Exception e) {
            //print("Erro");
            //print(e.Message);
        }

        return resultado;
    }
}
